namespace Swallow.Console.Arguments;

public sealed class CommandlineArgumentsTest
{
    [Test]
    public async Task UnixStyleFlags()
    {
        await AssertTokens("-b", Token.Option("b"));
        await AssertTokens("-abc", Token.Option("a"), Token.Option("b"), Token.Option("c"));
        await AssertTokens("--bar", Token.Option("bar"));
    }

    [Test]
    public async Task UnixStyleOptions()
    {
        await AssertTokens("-b foo", Token.Option("b"), Token.ParameterOrOptionValue("foo"));
        await AssertTokens("-abc foo", Token.Option("a"), Token.Option("b"), Token.Option("c"), Token.ParameterOrOptionValue("foo"));
        await AssertTokens("--bar foo", Token.Option("bar"), Token.ParameterOrOptionValue("foo"));
        await AssertTokens("--bar=foo", Token.Option("bar"), Token.OptionValue("foo"));
        await AssertTokens("--bar=foo", Token.Option("bar"), Token.OptionValue("foo"));
    }

    [Test]
    public async Task WindowsStyleFlags()
    {
        await AssertTokens("/b", Token.Option("b"));
        await AssertTokens("/bar", Token.Option("bar"));
    }

    [Test]
    public async Task WindowsStyleOptions()
    {
        await AssertTokens("/b:foo", Token.Option("b"), Token.OptionValue("foo"));
        await AssertTokens("/bar:foo", Token.Option("bar"), Token.OptionValue("foo"));
    }

    [Test]
    public async Task DoubleDashLimiter()
    {
        await AssertTokens("--foo -- bar", Token.Option("foo"), Token.Parameter("bar"));
        await AssertTokens("--foo -- --bar", Token.Option("foo"), Token.Parameter("--bar"));
    }

    [Test]
    public async Task QuotedParameters()
    {
        await AssertTokens(@"foo ""bar baz"" quuz", Token.Parameter("foo"), Token.Parameter("bar baz"), Token.Parameter("quuz"));
        await AssertTokens("foo 'bar baz' quuz", Token.Parameter("foo"), Token.Parameter("bar baz"), Token.Parameter("quuz"));
        await AssertTokens("mixed 'quotes \" here'", Token.Parameter("mixed"), Token.Parameter("quotes \" here"));
        await AssertTokens(@"mixed ""quotes ' here""", Token.Parameter("mixed"), Token.Parameter("quotes ' here"));
    }

    [Test]
    public async Task EscapedCharacters()
    {
        await AssertTokens(@"foo bar\ baz quuz", Token.Parameter("foo"), Token.Parameter("bar baz"), Token.Parameter("quuz"));
        await AssertTokens(@"'quote \' character'", Token.Parameter("quote \' character"));
        await AssertTokens(@"'quote \"" character'", Token.Parameter("quote \" character"));

        await AssertTokens(@"'Just a \n'", Token.Parameter("Just a \n"));
        await AssertTokens(@"'Just a \r'", Token.Parameter("Just a \r"));
        await AssertTokens(@"'Just a \t'", Token.Parameter("Just a \t"));
    }

    private static async Task AssertTokens(string input, params Token[] tokens)
    {
        var parsed = Arguments.Parse(input);
        await Assert.That(parsed).IsEquivalentTo(tokens);
    }
}
