namespace Swallow.Cli.Routing;

/// <summary>
/// Contains a list of <see cref="IEntrypointDataSource"/>, each providing its own <see cref="IEntrypoint"/>s.
/// </summary>
public interface IEntrypointRouteBuilder
{
    /// <summary>
    /// Gets a list of all configured <see cref="IEntrypointDataSource"/>s.
    /// </summary>
    public ICollection<IEntrypointDataSource> DataSources { get; }

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> used to resolve services.
    /// </summary>
    public IServiceProvider Services { get; }
}

/// <summary>
/// Quality of life extensions for the <see cref="IEntrypointRouteBuilder"/>.
/// </summary>
public static class EntrypointRouteBuilderExtensions
{
    /// <summary>
    /// Return the existing data source of type <typeparamref name="T"/> or create a new one.
    /// </summary>
    /// <param name="app">The <see cref="IEntrypointRouteBuilder"/> to which to add the data source.</param>
    /// <typeparam name="T">Type of the data source to add.</typeparam>
    /// <returns>The existing or newly created data source.</returns>
    public static T GetOrAddDataSource<T>(this IEntrypointRouteBuilder app) where T : IEntrypointDataSource, new()
    {
        return app.GetOrAddDataSource(static _ => new T());
    }

    /// <summary>
    /// Return the existing data source of type <typeparamref name="T"/> or create a new one.
    /// </summary>
    /// <param name="app">The <see cref="IEntrypointRouteBuilder"/> to which to add the data source.</param>
    /// <param name="factory">The function to call to create an instance of <typeparamref name="T"/>.</param>
    /// <typeparam name="T">Type of the data source to add.</typeparam>
    /// <returns>The existing or newly created data source.</returns>
    public static T GetOrAddDataSource<T>(this IEntrypointRouteBuilder app, Func<IServiceProvider, T> factory) where T : IEntrypointDataSource
    {
        var existingDataSource = app.DataSources.OfType<T>().FirstOrDefault();
        if (existingDataSource is not null)
        {
            return existingDataSource;
        }

        var dataSource = factory(app.Services);
        app.DataSources.Add(dataSource);

        return dataSource;
    }
}
