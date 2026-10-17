namespace Swallow.Console.Invocation;

public interface IMiddleware
{
    public Task InvokeAsync(InvocationContext invocationContext, InvocationHandler next);
}
