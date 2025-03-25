using ItFaker.Common;

namespace ItFaker.Tests;

public class StringFormattingTests
{
    [Fact]
    public void FormatItalianName_WithLowerCase_ShouldCapitalizeCorrectly()
    {
        // Arrange
        string lowerCaseName = "mario rossi";

        // Act
        string result = StringFormatting.FormatItalianName(lowerCaseName);

        // Assert
        Assert.Equal("Mario Rossi", result);
    }

    [Fact]
    public void FormatItalianName_WithUpperCase_ShouldFormatCorrectly()
    {
        // Arrange
        string upperCaseName = "GIUSEPPE VERDI";

        // Act
        string result = StringFormatting.FormatItalianName(upperCaseName);

        // Assert
        Assert.Equal("Giuseppe Verdi", result);
    }

    [Fact]
    public void FormatItalianName_WithMixedCase_ShouldFormatCorrectly()
    {
        // Arrange
        string mixedCaseName = "aNtOnIo bIaNcHi";

        // Act
        string result = StringFormatting.FormatItalianName(mixedCaseName);

        // Assert
        Assert.Equal("Antonio Bianchi", result);
    }

    [Fact]
    public void FormatItalianName_WithEmptyString_ShouldReturnEmpty()
    {
        // Arrange
        string emptyName = "";

        // Act
        string result = StringFormatting.FormatItalianName(emptyName);

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void FormatItalianName_WithNull_ShouldReturnNull()
    {
        // Arrange
        string? nullName = null;

        // Act
        string result = StringFormatting.FormatItalianName(nullName!);

        // Assert
        Assert.Null(result);
    }
}
