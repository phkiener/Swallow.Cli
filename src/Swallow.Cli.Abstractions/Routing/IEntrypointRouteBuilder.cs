namespace Swallow.Cli.Abstractions.Routing;

/// <summary>
/// Contains a list of <see cref="IEntrypointDataSource"/>, each providing its own <see cref="IEntrypoint"/>s.
/// </summary>
public interface IEntrypointRouteBuilder
{
    /// <summary>
    /// Gets or adds the datasource of type <typeparamref name="T"/> to the <see cref="IEntrypointRouteBuilder"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IEntrypointDataSource"/> to use.</typeparam>
    /// <returns>The new or existing <see cref="IEntrypointDataSource"/>.</returns>
    public T GetOrAddDataSource<T>() where T : IEntrypointDataSource, new();
}
