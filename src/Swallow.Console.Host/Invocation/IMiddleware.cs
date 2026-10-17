namespace Swallow.Console.Host.Invocation;

public interface IMiddleware
{
    public Task InvokeAsync(InvocationContext invocationContext, InvocationHandler next);
}
