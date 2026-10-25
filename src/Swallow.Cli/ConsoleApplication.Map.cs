using Swallow.Cli.Routing;

namespace Swallow.Cli;

partial class ConsoleApplication : IEntrypointRouteBuilder
{
    private readonly List<IEntrypointDataSource> dataSources = [];

    /// <summary>
    /// Map an entrypoint based on the given <see cref="Delegate"/>.
    /// </summary>
    /// <param name="handler">The delegate to map as <see cref="IEntrypoint"/>.</param>
    public void Map(Delegate handler)
    {
        entrypoints.Add(DelegateEntrypoint.Create(handler));
    }

    /// <summary>
    /// Map an entrypoint based on the given <see cref="Delegate"/>.
    /// </summary>
    /// <param name="command">The command prefix which acticates the command.</param>
    /// <param name="handler">The delegate to map as <see cref="IEntrypoint"/>.</param>
    public void Map(string command, Delegate handler)
    {
        entrypoints.Add(DelegateEntrypoint.Create([new Command(command)], handler));
    }

    private sealed record Command(string Name) : ICommandData;

    /// <inheritdoc />
    public T GetOrAddDataSource<T>() where T : IEntrypointDataSource, new()
    {
        var dataSource = dataSources.OfType<T>().FirstOrDefault();
        if (dataSource == null)
        {
            dataSource = new T();
            dataSources.Add(dataSource);
        }

        return dataSource;
    }
}
