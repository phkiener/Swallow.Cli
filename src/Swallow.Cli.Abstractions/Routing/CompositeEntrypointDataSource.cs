namespace Swallow.Cli.Routing;

/// <summary>
/// Bundles one or more <see cref="IEntrypointDataSource"/>s into a single data source.
/// </summary>
/// <param name="dataSources">The contained data sources.</param>
public sealed class CompositeEntrypointDataSource(IReadOnlyList<IEntrypointDataSource> dataSources) : IEntrypointDataSource
{
    /// <summary>
    /// The list of contained <see cref="IEntrypointDataSource"/>s.
    /// </summary>
    public IReadOnlyList<IEntrypointDataSource> DataSources => dataSources;

    /// <inheritdoc/>
    public IReadOnlyCollection<IEntrypoint> GetEntrypoints()
    {
        return DataSources.SelectMany(static ds => ds.GetEntrypoints()).ToList();
    }
}
