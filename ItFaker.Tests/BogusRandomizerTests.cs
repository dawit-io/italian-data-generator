using Bogus;
using ItFaker.Common;

namespace ItFaker.Tests;

public class BogusRandomizerTests
{
    [Fact]
    public void Constructor_WithNullRandomizer_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new BogusRandomizer(null!));
    }

    [Fact]
    public void Number_ShouldReturnValueInRange()
    {
        // Arrange
        var bogusRandomizer = new BogusRandomizer(new Randomizer());
        int min = 1;
        int max = 10;

        // Act
        int result = bogusRandomizer.Number(min, max);

        // Assert
        Assert.True(result >= min && result <= max);
    }

    [Fact]
    public void ArrayElement_ShouldReturnElementFromArray()
    {
        // Arrange
        var bogusRandomizer = new BogusRandomizer(new Randomizer());
        string[] testArray = new[] { "one", "two", "three" };

        // Act
        string result = bogusRandomizer.ArrayElement(testArray);

        // Assert
        Assert.Contains(result, testArray);
    }

    [Fact]
    public void ArrayElement_WithEmptyArray_ShouldThrowException()
    {
        // Arrange
        var bogusRandomizer = new BogusRandomizer(new Randomizer());
        string[] emptyArray = Array.Empty<string>();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => bogusRandomizer.ArrayElement(emptyArray));
    }
}
