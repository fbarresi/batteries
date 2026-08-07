using batteries.Extensions;
using Shouldly;

namespace batteries.Tests;

public class StringExtensionsTests
{
    #region TrimAfter Tests

    [Test]
    public void TrimAfter_WithSeparatorPresent_ReturnsTrimmedString()
    {
        var input = "Hello,World";
        var result = input.TrimAfter(',');
        result.ShouldBe("Hello");
    }

    [Test]
    public void TrimAfter_WithSeparatorAtStart_ReturnsEmptyString()
    {
        var input = ",Hello";
        var result = input.TrimAfter(',');
        result.ShouldBe(string.Empty);
    }

    [Test]
    public void TrimAfter_WithSeparatorAtEnd_ReturnsAllButSeparator()
    {
        var input = "Hello,";
        var result = input.TrimAfter(',');
        result.ShouldBe("Hello");
    }

    [Test]
    public void TrimAfter_WithNoSeparator_ReturnsOriginalString()
    {
        var input = "HelloWorld";
        var result = input.TrimAfter(',');
        result.ShouldBe("HelloWorld");
    }

    [Test]
    public void TrimAfter_WithMultipleSeparators_ReturnsTrimmedAtFirstOccurrence()
    {
        var input = "Hello,World,Test";
        var result = input.TrimAfter(',');
        result.ShouldBe("Hello");
    }

    [Test]
    public void TrimAfter_WithEmptyString_ReturnsEmptyString()
    {
        var input = "";
        var result = input.TrimAfter(',');
        result.ShouldBe(string.Empty);
    }

    [Test]
    public void TrimAfter_WithJustSeparator_ReturnsEmptyString()
    {
        var input = ",";
        var result = input.TrimAfter(',');
        result.ShouldBe(string.Empty);
    }

    [Test]
    public void TrimAfter_WithDifferentSeparators_ReturnsTrimmedAtSpecifiedSeparator()
    {
        var input = "Hello;World,Test";
        var result = input.TrimAfter(';');
        result.ShouldBe("Hello");
    }

    [Test]
    [TestCase("path/to/file.txt", '/')]
    [TestCase("path/to/file.txt", 'o')]
    [TestCase("domain.com", '.')]
    [TestCase("user@email.com", '@')]
    public void TrimAfter_WithVariousInputs_ReturnsCorrectTrimmedString(string input, char separator)
    {
        var result = input.TrimAfter(separator);
        result.ShouldNotContain(separator);
    }

    [Test]
    public void TrimAfter_WithWhitespaceCharacters_ReturnsTrimmedString()
    {
        var input = "Hello World";
        var result = input.TrimAfter(' ');
        result.ShouldBe("Hello");
    }

    [Test]
    public void TrimAfter_WithSpecialCharacters_ReturnsTrimmedString()
    {
        var input = "user:password";
        var result = input.TrimAfter(':');
        result.ShouldBe("user");
    }

    [Test]
    public void TrimAfter_WithNumericSeparator_ReturnsTrimmedString()
    {
        var input = "abc123def";
        var result = input.TrimAfter('1');
        result.ShouldBe("abc");
    }

    [Test]
    public void TrimAfter_WithSeparatorAsFirstCharacter_ReturnsEmptyString()
    {
        var input = "|rest of string";
        var result = input.TrimAfter('|');
        result.ShouldBe(string.Empty);
    }

    [Test]
    public void TrimAfter_WithLongString_ReturnsTrimmedCorrectly()
    {
        var input = "This is a very long string with comma,in the middle and more text";
        var result = input.TrimAfter(',');
        result.ShouldBe("This is a very long string with comma");
    }

    [Test]
    public void TrimAfter_WithUrlPath_ExtractsProtocol()
    {
        var input = "https://example.com";
        var result = input.TrimAfter(':');
        result.ShouldBe("https");
    }

    [Test]
    public void TrimAfter_WithCsvFormat_ExtractsFirstField()
    {
        var input = "John,Doe,30,Engineer";
        var result = input.TrimAfter(',');
        result.ShouldBe("John");
    }

    #endregion
}
