namespace Swallow.Cli.Invocation;

public interface IMiddleware
{
    public Task InvokeAsync(InvocationContext invocationContext, InvocationHandler next);
}
