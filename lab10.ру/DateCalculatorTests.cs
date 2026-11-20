using DateCalc.Core;
using Xunit;

namespace DateCalc.Tests;

public class DateCalculatorTests
{
    // ---------- DaysBetween ----------

    [Fact]
    public void DaysBetween_SameDate_ReturnsZero()
    {
        var date = new DateTime(2026, 11, 14);

        Assert.Equal(0, DateCalculator.DaysBetween(date, date));
    }

    [Fact]
    public void DaysBetween_NextDay_ReturnsOne()
    {
        var result = DateCalculator.DaysBetween(new DateTime(2026, 11, 14), new DateTime(2026, 11, 15));

        Assert.Equal(1, result);
    }

    [Fact]
    public void DaysBetween_SecondDateEarlier_ReturnsNegative()
    {
        var result = DateCalculator.DaysBetween(new DateTime(2026, 11, 15), new DateTime(2026, 11, 14));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void DaysBetween_LeapYear_Returns366()
    {
        var result = DateCalculator.DaysBetween(new DateTime(2024, 1, 1), new DateTime(2025, 1, 1));

        Assert.Equal(366, result);
    }

    [Fact]
    public void DaysBetween_IgnoresTimeOfDay()
    {
        // 23:59 и 00:01 следующего дня - всё равно один календарный день разницы
        var result = DateCalculator.DaysBetween(new DateTime(2026, 5, 1, 23, 59, 0), new DateTime(2026, 5, 2, 0, 1, 0));

        Assert.Equal(1, result);
    }

    // ---------- AddDays ----------

    [Fact]
    public void AddDays_PositiveAcrossLeapDay_GivesFirstOfMarch()
    {
        var result = DateCalculator.AddDays(new DateTime(2024, 2, 28), 2);

        Assert.Equal(new DateTime(2024, 3, 1), result);
    }

    [Fact]
    public void AddDays_Negative_GivesLeapDay()
    {
        var result = DateCalculator.AddDays(new DateTime(2024, 3, 1), -1);

        Assert.Equal(new DateTime(2024, 2, 29), result);
    }

    [Fact]
    public void AddDays_Zero_ReturnsSameDate()
    {
        var date = new DateTime(2026, 11, 14);

        Assert.Equal(date, DateCalculator.AddDays(date, 0));
    }

    [Fact]
    public void AddDays_BeyondMaxDate_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => DateCalculator.AddDays(DateTime.MaxValue, 1));
    }

    // ---------- GetDayOfWeekName ----------

    [Theory]
    [InlineData(2000, 1, 1, "суббота")]
    [InlineData(2024, 2, 29, "четверг")]
    [InlineData(2023, 12, 31, "воскресенье")]
    [InlineData(2024, 1, 1, "понедельник")]
    public void GetDayOfWeekName_KnownDates_ReturnsRussianName(int year, int month, int day, string expected)
    {
        var result = DateCalculator.GetDayOfWeekName(new DateTime(year, month, day));

        Assert.Equal(expected, result);
    }

    // ---------- IsLeapYear ----------

    [Theory]
    [InlineData(2024, true)]
    [InlineData(2023, false)]
    [InlineData(2000, true)]   // делится на 400
    [InlineData(1900, false)]  // делится на 100, но не на 400
    public void IsLeapYear_ReturnsExpected(int year, bool expected)
    {
        Assert.Equal(expected, DateCalculator.IsLeapYear(year));
    }

    [Fact]
    public void IsLeapYear_YearZero_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => DateCalculator.IsLeapYear(0));
    }

    // ---------- TryParseDate ----------

    [Fact]
    public void TryParseDate_CorrectString_ReturnsTrue()
    {
        bool ok = DateCalculator.TryParseDate("14.11.2026", out DateTime date);

        Assert.True(ok);
        Assert.Equal(new DateTime(2026, 11, 14), date);
    }

    [Theory]
    [InlineData("31.02.2024")]   // такого дня нет
    [InlineData("2024-02-29")]   // не тот формат
    [InlineData("abc")]
    [InlineData("")]
    [InlineData(null)]
    public void TryParseDate_BadString_ReturnsFalse(string? text)
    {
        Assert.False(DateCalculator.TryParseDate(text, out _));
    }
}
