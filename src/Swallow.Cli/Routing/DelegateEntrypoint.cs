using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
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
    /// Extract an <see cref="DelegateEntrypoint"/> based on the given <paramref name="handler"/>.
    /// The <see cref="RouteParts"/> are extracted using the attributes on the parameters for the method.
    /// </summary>
    /// <param name="handler">The underlying method to invoke.</param>
    /// <returns>The built <see cref="DelegateEntrypoint"/>.</returns>
    public static DelegateEntrypoint Create(Delegate handler) => Create([], handler);

    /// <summary>
    /// Extract an <see cref="DelegateEntrypoint"/> based on the given <paramref name="handler"/>.
    /// The <see cref="RouteParts"/> are extracted using the attributes on the parameters for the method.
    /// </summary>
    /// <param name="prefix">A prefix for the <see cref="RouteParts"/>.</param>
    /// <param name="handler">The underlying method to invoke.</param>
    /// <returns>The built <see cref="DelegateEntrypoint"/>.</returns>
    public static DelegateEntrypoint Create(ICommandData[] prefix, Delegate handler)
    {
        var parts = new List<IRoutePart>();
        foreach (var parameter in handler.Method.GetParameters())
        {
            var attributes = parameter.GetCustomAttributes(inherit: true);

            var flag = attributes.OfType<IFlagData>().FirstOrDefault();
            if (flag is not null)
            {
                parts.Add(flag);
                continue;
            }

            var option = attributes.OfType<IOptionData>().FirstOrDefault();
            if (option is not null)
            {
                parts.Add(option);
                continue;
            }

            var argument = attributes.OfType<IArgumentData>().FirstOrDefault();
            if (argument is not null)
            {
                parts.Add(argument);
                continue;
            }
        }

        // TODO: Handle overlap between flags and options; that's not allowed.

        return new DelegateEntrypoint([..prefix, ..parts], handler);
    }

    private Task InvokeDelegate(ConsoleContext consoleContext)
    {
        var parameters = underlyingHandler.Method.GetParameters();
        var arguments = new object?[parameters.Length];

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

            var option = attributes.OfType<IOptionData>().FirstOrDefault();
            if (option is not null)
            {
                throw new NotImplementedException("Options not handled yet.");
            }

            var argument = attributes.OfType<IArgumentData>().FirstOrDefault();
            if (argument is not null)
            {
                throw new NotImplementedException("Arguments not handled yet.");
            }

            // TODO: FromServices and FromKeyedServices are kinda duplicated...
            var fromServices = attributes.OfType<FromServicesAttribute>().FirstOrDefault();
            if (fromServices is not null)
            {
                var resolvedService = consoleContext.Services.GetService(type);
                if (resolvedService is not null)
                {
                    arguments[i] = resolvedService;
                    continue;
                }

                if (parameter.HasDefaultValue)
                {
                    arguments[i] = parameter.DefaultValue;
                    continue;
                }

                throw new InvalidOperationException($"[FromServices] parameter of type {type} not defined in the {nameof(IServiceProvider)} and no default value is provided.");
            }

            var fromKeyedServices = attributes.OfType<FromKeyedServicesAttribute>().FirstOrDefault();
            if (fromKeyedServices is not null)
            {
                var resolvedService = fromKeyedServices.LookupMode is ServiceKeyLookupMode.ExplicitKey
                    ? consoleContext.Services.GetKeyedService(type, fromKeyedServices.Key)
                    : consoleContext.Services.GetService(type);

                if (resolvedService is not null)
                {
                    arguments[i] = resolvedService;
                    continue;
                }

                if (parameter.HasDefaultValue)
                {
                    arguments[i] = parameter.DefaultValue;
                    continue;
                }

                throw new InvalidOperationException($"[FromKeyedServices] parameter of type {type} not defined in the {nameof(IServiceProvider)} and no default value is provided.");
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
