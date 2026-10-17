namespace Swallow.Console.Host.Routing;

public interface IFlagData
{
    bool IsRequired { get; }

    public char ShortName { get; }
    public string? LongName { get; }
    public string[] Aliases { get; }
}
