using Swallow.Cli.Abstractions.Routing;

namespace Swallow.Cli.Routing.Delegates;

internal sealed class DelegateParser
{
    public IRoutePart[] ParseRoute(Delegate @delegate)
    {
        var parts = new List<IRoutePart>();
        foreach (var parameter in @delegate.Method.GetParameters())
        {
            var attributes = parameter.GetCustomAttributes(inherit: true);

            var flag = attributes.OfType<IFlagData>().FirstOrDefault();
            if (flag is not null)
            {
                parts.Add(flag);
                continue;
            }

            var option = attributes.OfType<IOptionData>().FirstOrDefault();
            if (option is not null)
            {
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

        // TODO: Validate route parts; no overlap allowed between flags and options.

        return parts.ToArray();
    }
}
