using Microsoft.Extensions.DependencyInjection;
using Swallow.Cli.Invocation;

namespace Swallow.Cli.Routing;

public sealed class Entrypoint(InvocationHandler handler, RoutePattern routePattern)
{
    public RoutePattern RoutePattern { get; } = routePattern;
    public InvocationHandler Handler { get; } = handler;

    public static Entrypoint Parse(Delegate handler)
    {
        var flags = new List<IFlagData>();

        var parameters = handler.Method.GetParameters();
        var resolvers = new List<Func<InvocationContext, object?>>(capacity: parameters.Length);

        foreach (var parameter in parameters)
        {
            var attributes = parameter.GetCustomAttributes(inherit: true);
            var flag = attributes.OfType<IFlagData>().FirstOrDefault();
            if (flag is not null)
            {
                flags.Add(flag);
                resolvers.Add(c => c.Flags?.GetValueOrDefault(flag) ?? false);

                continue;
            }

            var injectAttribute = attributes.OfType<FromServicesAttribute>().FirstOrDefault();
            if (injectAttribute is not null)
            {
                resolvers.Add(c => injectAttribute.Key is null ? c.ServiceProvider.GetService(parameter.ParameterType) : c.ServiceProvider.GetKeyedService(parameter.ParameterType, injectAttribute.Key));
            }

            if (parameter.ParameterType == typeof(InvocationContext))
            {
                resolvers.Add(static c => c);

                continue;
            }

            if (parameter.ParameterType == typeof(CancellationToken))
            {
                resolvers.Add(static c => c.CancellationToken);

                continue;
            }

            if (parameter.ParameterType == typeof(IServiceProvider))
            {
                resolvers.Add(static c => c.ServiceProvider);

                continue;
            }

            throw new NotImplementedException($"Cannot resolve parameter {parameter}");
        }

        return new Entrypoint(ctx =>
            {
                var args = new List<object?>();
                foreach (var resolver in resolvers)
                {
                    args.Add(resolver.Invoke(ctx));
                }

                var result = handler.DynamicInvoke([.. args]);
                return result as Task ?? Task.CompletedTask;
            },
            new RoutePattern([.. flags]));
    }
}
