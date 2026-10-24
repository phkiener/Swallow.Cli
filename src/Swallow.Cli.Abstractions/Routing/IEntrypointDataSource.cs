namespace Swallow.Cli.Abstractions.Routing;

/// <summary>
/// A data source containing one or more <see cref="IEntrypoint"/>s.
/// </summary>
public interface IEntrypointDataSource
{
    /// <summary>
    /// Returns all contained <see cref="IEntrypoint"/>s.
    /// </summary>
    public IReadOnlyCollection<IEntrypoint> GetEntrypoints();
}
