using Shouldly;
using TFU002.Interfaces.Extensions;

namespace batteries.Tests;

public class TimeSpanExtensionsTests
{
    #region AtLeast Tests

    [Test]
    public void AtLeast_WhenTimeSpanIsGreaterThanMinimal_ReturnsOriginalTimeSpan()
    {
        var timeSpan = TimeSpan.FromSeconds(10);
        var minimal = TimeSpan.FromSeconds(5);
        
        var result = timeSpan.AtLeast(minimal);
        
        result.ShouldBe(TimeSpan.FromSeconds(10));
    }

    [Test]
    public void AtLeast_WhenTimeSpanIsEqualToMinimal_ReturnsTimeSpan()
    {
        var timeSpan = TimeSpan.FromSeconds(5);
        var minimal = TimeSpan.FromSeconds(5);
        
        var result = timeSpan.AtLeast(minimal);
        
        result.ShouldBe(TimeSpan.FromSeconds(5));
    }

    [Test]
    public void AtLeast_WhenTimeSpanIsLessThanMinimal_ReturnsMinimalTimeSpan()
    {
        var timeSpan = TimeSpan.FromSeconds(3);
        var minimal = TimeSpan.FromSeconds(5);
        
        var result = timeSpan.AtLeast(minimal);
        
        result.ShouldBe(TimeSpan.FromSeconds(5));
    }

    [Test]
    public void AtLeast_WithZeroTimeSpan_ReturnsMinimalTimeSpan()
    {
        var timeSpan = TimeSpan.Zero;
        var minimal = TimeSpan.FromSeconds(5);
        
        var result = timeSpan.AtLeast(minimal);
        
        result.ShouldBe(TimeSpan.FromSeconds(5));
    }

    [Test]
    public void AtLeast_WithZeroMinimal_ReturnsOriginalTimeSpan()
    {
        var timeSpan = TimeSpan.FromSeconds(5);
        var minimal = TimeSpan.Zero;
        
        var result = timeSpan.AtLeast(minimal);
        
        result.ShouldBe(TimeSpan.FromSeconds(5));
    }

    [Test]
    public void AtLeast_WithBothZero_ReturnsZero()
    {
        var timeSpan = TimeSpan.Zero;
        var minimal = TimeSpan.Zero;
        
        var result = timeSpan.AtLeast(minimal);
        
        result.ShouldBe(TimeSpan.Zero);
    }

    [Test]
    public void AtLeast_WithMilliseconds_WorksCorrectly()
    {
        var timeSpan = TimeSpan.FromMilliseconds(100);
        var minimal = TimeSpan.FromMilliseconds(200);
        
        var result = timeSpan.AtLeast(minimal);
        
        result.ShouldBe(TimeSpan.FromMilliseconds(200));
    }

    [Test]
    public void AtLeast_WithMinutes_WorksCorrectly()
    {
        var timeSpan = TimeSpan.FromMinutes(2);
        var minimal = TimeSpan.FromMinutes(5);
        
        var result = timeSpan.AtLeast(minimal);
        
        result.ShouldBe(TimeSpan.FromMinutes(5));
    }

    [Test]
    public void AtLeast_WithHours_WorksCorrectly()
    {
        var timeSpan = TimeSpan.FromHours(1);
        var minimal = TimeSpan.FromHours(2);
        
        var result = timeSpan.AtLeast(minimal);
        
        result.ShouldBe(TimeSpan.FromHours(2));
    }

    [Test]
    public void AtLeast_WithDays_WorksCorrectly()
    {
        var timeSpan = TimeSpan.FromDays(5);
        var minimal = TimeSpan.FromDays(3);
        
        var result = timeSpan.AtLeast(minimal);
        
        result.ShouldBe(TimeSpan.FromDays(5));
    }

    [Test]
    public void AtLeast_WithNegativeTimeSpan_ReturnsMinimalIfGreater()
    {
        var timeSpan = TimeSpan.FromSeconds(-5);
        var minimal = TimeSpan.FromSeconds(5);
        
        var result = timeSpan.AtLeast(minimal);
        
        result.ShouldBe(TimeSpan.FromSeconds(5));
    }

    [Test]
    public void AtLeast_WithNegativeMinimal_ReturnsOriginalIfGreater()
    {
        var timeSpan = TimeSpan.FromSeconds(5);
        var minimal = TimeSpan.FromSeconds(-10);
        
        var result = timeSpan.AtLeast(minimal);
        
        result.ShouldBe(TimeSpan.FromSeconds(5));
    }

    [Test]
    public void AtLeast_WithBothNegative_ReturnsMinimalIfSmaller()
    {
        var timeSpan = TimeSpan.FromSeconds(-10);
        var minimal = TimeSpan.FromSeconds(-5);
        
        var result = timeSpan.AtLeast(minimal);
        
        result.ShouldBe(TimeSpan.FromSeconds(-5));
    }

    [Test]
    public void AtLeast_WithMaxValue_ReturnsMaxValue()
    {
        var timeSpan = TimeSpan.MaxValue;
        var minimal = TimeSpan.FromSeconds(5);
        
        var result = timeSpan.AtLeast(minimal);
        
        result.ShouldBe(TimeSpan.MaxValue);
    }

    #endregion
}
