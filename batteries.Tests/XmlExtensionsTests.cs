using batteries.Extensions;
using Shouldly;

namespace batteries.Tests;

public class XmlExtensionsTests
{
    #region ExtractValueFromXmlQuery Tests

    [Test]
    public void ExtractValueFromXmlQuery_WithSimpleElement_ReturnsValue()
    {
        var xml = "<root><node>value</node></root>";
        var result = xml.ExtractValueFromXmlQuery("/root/node");
        result.ShouldBe("value");
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithNestedElements_ReturnsValue()
    {
        var xml = "<root><parent><child>nested value</child></parent></root>";
        var result = xml.ExtractValueFromXmlQuery("/root/parent/child");
        result.ShouldBe("nested value");
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithNonExistentPath_ReturnsEmptyString()
    {
        var xml = "<root><node>value</node></root>";
        var result = xml.ExtractValueFromXmlQuery("/root/nonexistent");
        result.ShouldBe(string.Empty);
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithInvalidXml_ReturnsEmptyString()
    {
        var xml = "<root><node>value</root>";
        var result = xml.ExtractValueFromXmlQuery("/root/node");
        result.ShouldBe(string.Empty);
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithEmptyXml_ReturnsEmptyString()
    {
        var xml = "";
        var result = xml.ExtractValueFromXmlQuery("/root/node");
        result.ShouldBe(string.Empty);
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithEmptyNode_ReturnsEmptyString()
    {
        var xml = "<root><node></node></root>";
        var result = xml.ExtractValueFromXmlQuery("/root/node");
        result.ShouldBe(string.Empty);
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithNumericValue_ReturnsStringValue()
    {
        var xml = "<root><number>42</number></root>";
        var result = xml.ExtractValueFromXmlQuery("/root/number");
        result.ShouldBe("42");
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithWhitespace_ReturnsValueWithoutTrimming()
    {
        var xml = "<root><node>  value  </node></root>";
        var result = xml.ExtractValueFromXmlQuery("/root/node");
        result.ShouldContain("value");
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithSpecialCharacters_ReturnsValue()
    {
        var xml = "<root><node>&lt;special&gt;</node></root>";
        var result = xml.ExtractValueFromXmlQuery("/root/node");
        result.ShouldBe("<special>");
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithAttributes_IgnoresAttributesAndReturnsInnerText()
    {
        var xml = "<root><node attr=\"test\">value</node></root>";
        var result = xml.ExtractValueFromXmlQuery("/root/node");
        result.ShouldBe("value");
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithMultipleSiblings_ReturnFirstMatch()
    {
        var xml = "<root><node>first</node><node>second</node></root>";
        var result = xml.ExtractValueFromXmlQuery("/root/node");
        result.ShouldBe("first");
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithXmlDeclaration_ReturnsValue()
    {
        var xml = "<?xml version=\"1.0\"?><root><node>value</node></root>";
        var result = xml.ExtractValueFromXmlQuery("/root/node");
        result.ShouldBe("value");
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithNamespace_ReturnsEmptyStringWithoutNamespace()
    {
        var xml = "<root xmlns=\"http://example.com\"><node>value</node></root>";
        var result = xml.ExtractValueFromXmlQuery("/root/node");
        result.ShouldBe(string.Empty);
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithCDATA_ReturnsValue()
    {
        var xml = "<root><node><![CDATA[special <content> here]]></node></root>";
        var result = xml.ExtractValueFromXmlQuery("/root/node");
        result.ShouldContain("special <content> here");
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithRootQuery_ReturnsEmpty()
    {
        var xml = "<root></root>";
        var result = xml.ExtractValueFromXmlQuery("/");
        result.ShouldBe(string.Empty);
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithComplexPath_ReturnsCorrectValue()
    {
        var xml = @"<root>
            <parent1>
                <parent2>
                    <child>deep value</child>
                </parent2>
            </parent1>
        </root>";
        var result = xml.ExtractValueFromXmlQuery("/root/parent1/parent2/child");
        result.ShouldBe("deep value");
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithMultipleChildren_ReturnsCorrectNode()
    {
        var xml = "<root><node1>value1</node1><node2>value2</node2><node3>value3</node3></root>";
        var result = xml.ExtractValueFromXmlQuery("/root/node2");
        result.ShouldBe("value2");
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithNodeContainingChildElements_ReturnsInnerText()
    {
        var xml = "<root><node>before<child>child content</child>after</node></root>";
        var result = xml.ExtractValueFromXmlQuery("/root/node");
        result.ShouldContain("child content");
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithInvalidQuerySyntax_ReturnsEmptyString()
    {
        var xml = "<root><node>value</node></root>";
        var result = xml.ExtractValueFromXmlQuery("invalid//xpath//");
        result.ShouldBe(string.Empty);
    }

    [Test]
    public void ExtractValueFromXmlQuery_WithAbsoluteAndRelativePaths_ReturnsValueForAbsolutePath()
    {
        var xml = "<root><node>value</node></root>";
        var result = xml.ExtractValueFromXmlQuery("/root/node");
        result.ShouldBe("value");
    }

    #endregion
}
