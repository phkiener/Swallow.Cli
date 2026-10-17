using Swallow.Console.Invocation;

namespace Swallow.Console.Routing;

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
            var parameterFlags = attributes.OfType<IFlagData>().ToArray();
            if (parameterFlags is [var flag, ..])
            {
                flags.Add(flag);
                resolvers.Add(c => c.Flags?.GetValueOrDefault(flag) ?? false);
            }
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
