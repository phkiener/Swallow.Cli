using Swallow.Cli.Abstractions.Routing;

namespace Swallow.Cli.Routing.Delegates;

internal sealed class DelegateParser
{
    public IRoutePart[] ParseRoute(Delegate @delegate)
    {
        var seenShortNames = new HashSet<char>();
        var seenLongNames = new HashSet<string>();

        var parts = new List<IRoutePart>();
        foreach (var parameter in @delegate.Method.GetParameters())
        {
            var attributes = parameter.GetCustomAttributes(inherit: true);

            var flag = attributes.OfType<IFlagData>().FirstOrDefault();
            if (flag is not null)
            {
                if (flag.ShortName != '\0' && !seenShortNames.Add(flag.ShortName))
                {
                    throw new InvalidOperationException($"Conflict: Short name {flag.ShortName} is found more than once.");
                }

                if (flag.LongName is not null && !seenLongNames.Add(flag.LongName))
                {
                    throw new InvalidOperationException($"Conflict: Long name {flag.ShortName} is found more than once.");
                }

                parts.Add(flag);

                continue;
            }

            var option = attributes.OfType<IOptionData>().FirstOrDefault();
            if (option is not null)
            {
                if (option.ShortName != '\0' && !seenShortNames.Add(option.ShortName))
                {
                    throw new InvalidOperationException($"Conflict: Short name {option.ShortName} is found more than once.");
                }

                if (option.LongName is not null && !seenLongNames.Add(option.LongName))
                {
                    throw new InvalidOperationException($"Conflict: Long name {option.ShortName} is found more than once.");
                }

                parts.Add(option);
                continue;
            }

            var argument = attributes.OfType<IArgumentData>().FirstOrDefault();
            if (argument is not null)
            {
                parts.Add(argument);
                continue;
            }
        }

        return parts.ToArray();
    }
}
