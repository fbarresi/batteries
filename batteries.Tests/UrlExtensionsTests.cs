using batteries.Extensions;
using Shouldly;

namespace batteries.Tests;

public class UrlExtensionsTests
{
    #region IsValidWebUrl Tests

    [Test]
    public void IsValidWebUrl_WithValidHttpUrl_ReturnsTrue()
    {
        var url = "http://example.com";
        var result = url.IsValidWebUrl();
        result.ShouldBeTrue();
    }

    [Test]
    public void IsValidWebUrl_WithValidHttpsUrl_ReturnsTrue()
    {
        var url = "https://example.com";
        var result = url.IsValidWebUrl();
        result.ShouldBeTrue();
    }

    [Test]
    public void IsValidWebUrl_WithComplexHttpUrl_ReturnsTrue()
    {
        var url = "http://www.example.com/path/to/page";
        var result = url.IsValidWebUrl();
        result.ShouldBeTrue();
    }

    [Test]
    public void IsValidWebUrl_WithQueryParameters_ReturnsTrue()
    {
        var url = "https://example.com/search?q=test&page=1";
        var result = url.IsValidWebUrl();
        result.ShouldBeTrue();
    }

    [Test]
    public void IsValidWebUrl_WithFragment_ReturnsTrue()
    {
        var url = "https://example.com/page#section";
        var result = url.IsValidWebUrl();
        result.ShouldBeTrue();
    }

    [Test]
    public void IsValidWebUrl_WithPort_ReturnsTrue()
    {
        var url = "http://example.com:8080/path";
        var result = url.IsValidWebUrl();
        result.ShouldBeTrue();
    }

    [Test]
    public void IsValidWebUrl_WithAuthentication_ReturnsTrue()
    {
        var url = "http://user:password@example.com";
        var result = url.IsValidWebUrl();
        result.ShouldBeTrue();
    }

    [Test]
    public void IsValidWebUrl_WithoutScheme_ReturnsFalse()
    {
        var url = "example.com";
        var result = url.IsValidWebUrl();
        result.ShouldBeFalse();
    }

    [Test]
    public void IsValidWebUrl_WithFtpScheme_ReturnsFalse()
    {
        var url = "ftp://example.com";
        var result = url.IsValidWebUrl();
        result.ShouldBeFalse();
    }

    [Test]
    public void IsValidWebUrl_WithFileScheme_ReturnsFalse()
    {
        var url = "file:///C:/Users/test.txt";
        var result = url.IsValidWebUrl();
        result.ShouldBeFalse();
    }

    [Test]
    public void IsValidWebUrl_WithRelativePath_ReturnsFalse()
    {
        var url = "/path/to/page";
        var result = url.IsValidWebUrl();
        result.ShouldBeFalse();
    }

    [Test]
    public void IsValidWebUrl_WithEmptyString_ReturnsFalse()
    {
        var url = "";
        var result = url.IsValidWebUrl();
        result.ShouldBeFalse();
    }

    [Test]
    public void IsValidWebUrl_WithJustScheme_ReturnsFalse()
    {
        var url = "http://";
        var result = url.IsValidWebUrl();
        result.ShouldBeFalse();
    }

    [Test]
    public void IsValidWebUrl_WithInvalidCharacters_ReturnsFalse()
    {
        var url = "http://exa mple.com";
        var result = url.IsValidWebUrl();
        result.ShouldBeFalse();
    }

    [Test]
    public void IsValidWebUrl_WithMailtoScheme_ReturnsFalse()
    {
        var url = "mailto:test@example.com";
        var result = url.IsValidWebUrl();
        result.ShouldBeFalse();
    }

    [Test]
    [TestCase("http://localhost")]
    [TestCase("http://localhost:3000")]
    [TestCase("https://localhost:443/api")]
    public void IsValidWebUrl_WithLocalhost_ReturnsTrue(string url)
    {
        var result = url.IsValidWebUrl();
        result.ShouldBeTrue();
    }

    [Test]
    [TestCase("http://192.168.1.1")]
    [TestCase("http://192.168.1.1:8080")]
    [TestCase("https://10.0.0.1")]
    public void IsValidWebUrl_WithIpAddress_ReturnsTrue(string url)
    {
        var result = url.IsValidWebUrl();
        result.ShouldBeTrue();
    }

    [Test]
    public void IsValidWebUrl_WithSubdomain_ReturnsTrue()
    {
        var url = "https://api.v2.example.com/v1/users";
        var result = url.IsValidWebUrl();
        result.ShouldBeTrue();
    }

    [Test]
    public void IsValidWebUrl_WithTrailingSlash_ReturnsTrue()
    {
        var url = "https://example.com/";
        var result = url.IsValidWebUrl();
        result.ShouldBeTrue();
    }

    [Test]
    public void IsValidWebUrl_WithMultipleSlashes_ReturnsTrue()
    {
        var url = "https://example.com///path///to///page";
        var result = url.IsValidWebUrl();
        result.ShouldBeTrue();
    }

    #endregion
}
