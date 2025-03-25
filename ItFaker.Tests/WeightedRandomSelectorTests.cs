using ItFaker.Common;
using ItFaker.Models;

namespace ItFaker.Tests;

public class WeightedRandomSelectorTests
{
    [Fact]
    public void Constructor_WithNullItems_ShouldThrowException()
    {
        // Arrange
        List<WeightedItem<string>>? nullList = null;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new WeightedRandomSelector<string>(nullList!));
    }

    [Fact]
    public void Constructor_WithEmptyItems_ShouldThrowException()
    {
        // Arrange
        var emptyList = new List<WeightedItem<string>>();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new WeightedRandomSelector<string>(emptyList));
    }

    [Fact]
    public void Select_WithSingleItem_ShouldReturnThatItem()
    {
        // Arrange
        var items = new List<WeightedItem<string>>
            {
                new WeightedItem<string> { Item = "test", Weight = 100 }
            };
        var selector = new WeightedRandomSelector<string>(items);

        // Act
        string result = selector.Select();

        // Assert
        Assert.Equal("test", result);
    }

    [Fact]
    public void Select_WithMultipleItems_ShouldReturnOneOfThem()
    {
        // Arrange
        var items = new List<WeightedItem<string>>
            {
                new WeightedItem<string> { Item = "one", Weight = 100 },
                new WeightedItem<string> { Item = "two", Weight = 200 },
                new WeightedItem<string> { Item = "three", Weight = 300 }
            };
        var validResults = new[] { "one", "two", "three" };
        var selector = new WeightedRandomSelector<string>(items);

        // Act
        string result = selector.Select();

        // Assert
        Assert.Contains(result, validResults);
    }

    [Fact]
    public void Select_WithStronglyWeightedItem_ShouldFavorThatItem()
    {
        // Arrange - create an item with 99% weight and one with 1% weight
        var items = new List<WeightedItem<string>>
            {
                new WeightedItem<string> { Item = "rare", Weight = 1 },
                new WeightedItem<string> { Item = "common", Weight = 99 }
            };
        var selector = new WeightedRandomSelector<string>(items);

        // Act - select many times
        var results = new List<string>();
        for (int i = 0; i < 100; i++)
        {
            results.Add(selector.Select());
        }

        // Assert - "common" should appear much more frequently
        int commonCount = results.Count(r => r == "common");
        // We can't deterministically test randomness, but we can verify 
        // it's statistically likely to select the common item more often
        Assert.True(commonCount > 50, "The heavily weighted item should be selected more frequently");
    }
}
