using Swallow.Cli.Routing;

namespace Swallow.Console.Test.Routing;

public sealed class TokenizerTest
{
    [Test]
    public async Task EmptyArguments_ReturnsEmptyTokens()
    {
        var tokens = Tokenizer.Tokenize([]);
        await Assert.That(tokens).IsEmpty();
    }

    [Test]
    [Arguments("-f", "f")]
    [Arguments("-0", "0")]
    public async Task ShortFlag_ReturnsFlagToken(string input, string symbol)
    {
        var tokens = Tokenizer.Tokenize([input]);
        await Assert.That(tokens).IsEquivalentTo([Flag(symbol)]);
    }

    [Test]
    [Arguments("--foobar", "foobar")]
    public async Task LongFlag_ReturnsFlagToken(string input, string symbol)
    {
        var tokens = Tokenizer.Tokenize([input]);
        await Assert.That(tokens).IsEquivalentTo([Flag(symbol)]);
    }

    [Test]
    public async Task FirstToken_IsCommandOrArgument()
    {
        var tokens = Tokenizer.Tokenize(["foobar"]);
        await Assert.That(tokens).IsEquivalentTo([CommandOrArgument("foobar")]);
    }

    [Test]
    public async Task TokenAfterFlag_IsOptionArgumentOrArgument()
    {
        var tokens = Tokenizer.Tokenize(["-c", "foobar"]);
        await Assert.That(tokens).IsEquivalentTo([Flag("c"), OptionArgumentOrArgument("foobar")]);
    }

    [Test]
    public async Task AfterDoubleDash_EverythingIsAnArgument()
    {
        var tokens = Tokenizer.Tokenize(["--", "-f", "--bar", "value"]);
        await Assert.That(tokens).IsEquivalentTo([Argument("-f"), Argument("--bar"), Argument("value")]);
    }

    [Test]
    public async Task CompoundFlag_IsSplitApart()
    {
        var tokens = Tokenizer.Tokenize(["-abc", "value"]);
        await Assert.That(tokens).IsEquivalentTo([Flag("a"), Flag("b"), Flag("c"), OptionArgumentOrArgument("value")]);
    }

    private static CommandlineToken Flag(string content) => new(content, TokenType.Flag);
    private static CommandlineToken Argument(string content) => new(content, TokenType.Argument);
    private static CommandlineToken CommandOrArgument(string content) => new(content, TokenType.Command | TokenType.Argument);
    private static CommandlineToken OptionArgumentOrArgument(string content) => new(content, TokenType.OptionArgument | TokenType.Argument);
}
