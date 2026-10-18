using System.Diagnostics;
using Swallow.Cli.Abstractions;
using Swallow.Cli.Abstractions.Routing;

namespace Swallow.Cli.Routing;

/// <summary>
/// An <see cref="IEntrypoint"/> based on a delegate.
/// </summary>
public sealed class DelegateEntrypoint : IEntrypoint
{
    private readonly Delegate underlyingHandler;

    private DelegateEntrypoint(IRoutePart[] routeParts, Delegate handler)
    {
        underlyingHandler = handler;
        RouteParts = routeParts;
        Delegate = InvokeDelegate;
    }

    /// <inheritdoc/>
    public IRoutePart[] RouteParts { get; }

    /// <inheritdoc/>
    public InvocationDelegate Delegate { get; }

    /// <summary>
    /// Extract an <see cref="DelegateEntrypoint"/> based on the given <see cref="handler"/>.
    /// The <see cref="RouteParts"/> are extracted using the attributes on the parameters for the method.
    /// </summary>
    /// <param name="handler">The underlying method to invoke.</param>
    /// <returns>The built <see cref="DelegateEntrypoint"/>.</returns>
    public static DelegateEntrypoint Create(Delegate handler)
    {
        var flags = new List<IFlagData>();
        foreach (var parameter in handler.Method.GetParameters())
        {
            var attributes = parameter.GetCustomAttributes(inherit: true);
            var flag = attributes.OfType<IFlagData>().FirstOrDefault();

            if (flag is not null)
            {
                flags.Add(flag);
            }
        }

        return new DelegateEntrypoint([..flags], handler);
    }

    private Task InvokeDelegate(ConsoleContext consoleContext)
    {
        var parameters = underlyingHandler.Method.GetParameters();
        var arguments = new object[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];

            var attributes = parameter.GetCustomAttributes(inherit: true);
            var type = parameter.ParameterType;

            var flag = attributes.OfType<IFlagData>().FirstOrDefault();
            if (flag is not null)
            {
                Debug.Assert(type == typeof(bool) || type == typeof(bool?), "Flag parameter is a boolean");
                arguments[i] = consoleContext.RouteValues?.EnabledFlags.Contains(flag) ?? false;

                continue;
            }

            if (type == typeof(ConsoleContext))
            {
                arguments[i] = consoleContext;
                continue;
            }

            if (type == typeof(CancellationToken))
            {
                arguments[i] = consoleContext.Aborted;
                continue;
            }

            throw new InvalidOperationException($"Parameter of type {type} cannot be bound for delegate in {nameof(DelegateEntrypoint)}.");
        }

        var result = underlyingHandler.DynamicInvoke(arguments);
        return result as Task ?? Task.CompletedTask;
    }
}
