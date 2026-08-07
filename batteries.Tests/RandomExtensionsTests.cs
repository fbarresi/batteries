using batteries.Extensions;
using Shouldly;

namespace batteries.Tests;

public class RandomExtensionsTests
{
    private Random _random;

    [SetUp]
    public void Setup()
    {
        _random = new Random();
    }

    #region GetData Tests

    [Test]
    public void GetData_WithBoolType_ReturnsBoolean()
    {
        var result = _random.GetData<bool>();
        result.ShouldBeOfType<bool>();
    }

    [Test]
    public void GetData_WithByteType_ReturnsByte()
    {
        var result = _random.GetData<byte>();
        result.ShouldBeOfType<byte>();
    }

    [Test]
    public void GetData_WithSByteType_ReturnsSByte()
    {
        var result = _random.GetData<sbyte>();
        result.ShouldBeOfType<sbyte>();
    }

    [Test]
    public void GetData_WithCharType_ReturnsChar()
    {
        var result = _random.GetData<char>();
        result.ShouldBeOfType<char>();
    }

    [Test]
    public void GetData_WithInt16Type_ReturnsInt16()
    {
        var result = _random.GetData<short>();
        result.ShouldBeOfType<short>();
    }

    [Test]
    public void GetData_WithUInt16Type_ReturnsUInt16()
    {
        var result = _random.GetData<ushort>();
        result.ShouldBeOfType<ushort>();
    }

    [Test]
    public void GetData_WithInt32Type_ReturnsInt32()
    {
        var result = _random.GetData<int>();
        result.ShouldBeOfType<int>();
    }

    [Test]
    public void GetData_WithUInt32Type_ReturnsUInt32()
    {
        var result = _random.GetData<uint>();
        result.ShouldBeOfType<uint>();
    }

    [Test]
    public void GetData_WithInt64Type_ReturnsInt64()
    {
        var result = _random.GetData<long>();
        result.ShouldBeOfType<long>();
    }

    [Test]
    public void GetData_WithUInt64Type_ReturnsUInt64()
    {
        var result = _random.GetData<ulong>();
        result.ShouldBeOfType<ulong>();
    }

    [Test]
    public void GetData_WithSingleType_ReturnsSingle()
    {
        var result = _random.GetData<float>();
        result.ShouldBeOfType<float>();
    }

    [Test]
    public void GetData_WithDoubleType_ReturnsDouble()
    {
        var result = _random.GetData<double>();
        result.ShouldBeOfType<double>();
    }

    [Test]
    public void GetData_WithDecimalType_ReturnsDecimal()
    {
        var result = _random.GetData<decimal>();
        result.ShouldBeOfType<decimal>();
    }

    [Test]
    public void GetData_WithDateTimeType_ReturnsDateTime()
    {
        var result = _random.GetData<DateTime>();
        result.ShouldBeOfType<DateTime>();
    }

    [Test]
    public void GetData_WithStringType_ReturnsString()
    {
        var result = _random.GetData<string>();
        result.ShouldBeOfType<string>();
        result.ShouldNotBeNullOrEmpty();
    }

    [Test]
    public void GetData_WithStringType_ReturnsGuidString()
    {
        var result = _random.GetData<string>();
        result.ShouldNotBeNullOrEmpty();
        Guid.TryParse(result, out _).ShouldBeTrue();
    }

    [Test]
    public void GetData_WithBoolType_MultipleCallsReturnDifferentValues()
    {
        var results = Enumerable.Range(0, 10)
            .Select(_ => _random.GetData<bool>())
            .Distinct()
            .Count();

        results.ShouldBeGreaterThan(1);
    }

    [Test]
    public void GetData_WithIntType_MultipleCallsReturnDifferentValues()
    {
        var results = Enumerable.Range(0, 10)
            .Select(_ => _random.GetData<int>())
            .Distinct()
            .Count();

        results.ShouldBeGreaterThanOrEqualTo(5);
    }

    [Test]
    public void GetData_WithUnsupportedType_ReturnsDefault()
    {
        var result = _random.GetData<object>();
        result.ShouldBeNull();
    }

    #endregion

    #region GetString Tests

    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(5)]
    [TestCase(10)]
    [TestCase(100)]
    public void GetString_WithValidLength_ReturnsCorrectLength(int length)
    {
        var result = _random.GetString(length);
        result.Length.ShouldBe(length);
    }

    [Test]
    public void GetString_WithLength1_ReturnsSingleCharacter()
    {
        var result = _random.GetString(1);
        result.Length.ShouldBe(1);
    }

    [Test]
    public void GetString_WithLength0_ReturnsEmptyString()
    {
        var result = _random.GetString(0);
        result.ShouldBe(string.Empty);
    }

    [Test]
    public void GetString_WithLength10_ReturnsOnlyValidCharacters()
    {
        const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var result = _random.GetString(10);

        foreach (var c in result)
        {
            validChars.ShouldContain(c);
        }
    }

    [Test]
    public void GetString_MultipleCallsReturnDifferentValues()
    {
        var results = Enumerable.Range(0, 10)
            .Select(_ => _random.GetString(10))
            .Distinct()
            .Count();

        results.ShouldBeGreaterThan(1);
    }

    [Test]
    public void GetString_WithLength50_ReturnsValidString()
    {
        var result = _random.GetString(50);
        result.Length.ShouldBe(50); 
        result.ShouldNotBeNullOrEmpty();
        result.ShouldAllBe(c => char.IsLetterOrDigit(c));
    }

    #endregion
}
