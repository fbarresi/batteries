using System.Text.RegularExpressions;
using batteries.Extensions;
using Shouldly;

namespace batteries.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
    
    [Test]
    public void TestExtractValueFromXmlQuery()
    {
        var xml = @"<root><node1>value1</node1><node2>value2</node2></root>";
        var result = xml.ExtractValueFromXmlQuery("/root/node1");
        result.ShouldBe("value1");
    }
}