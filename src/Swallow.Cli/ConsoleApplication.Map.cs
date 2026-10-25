using Swallow.Cli.Routing;

namespace Swallow.Cli;

partial class ConsoleApplication : IEntrypointRouteBuilder
{
    private readonly List<IEntrypointDataSource> dataSources = [];

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
