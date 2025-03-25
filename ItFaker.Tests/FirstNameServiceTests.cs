using System.Reflection;
using ItFaker.Common;
using ItFaker.Models;
using ItFaker.Services;
using Moq;

namespace ItFaker.Tests;

public class FirstNameServiceTests
{
    private readonly Mock<IRandomizer> _randomizerMock;
    private readonly FirstNameService _firstNameService;

    public FirstNameServiceTests()
    {
        // Setup mock for IRandomizer instead of Randomizer
        _randomizerMock = new Mock<IRandomizer>();
        _firstNameService = new FirstNameService(_randomizerMock.Object);

        // Setup embedded resource reading by using reflection to set private fields
        SetupEmbeddedResourceMocking();
    }

    [Fact]
    public async Task GenerateAsync_WithMaleGender_ShouldReturnMaleName()
    {
        // Arrange
        _randomizerMock.Setup(r => r.Number(It.IsAny<int>(), It.IsAny<int>())).Returns(0); // 0 = Male
        _randomizerMock.Setup(r => r.ArrayElement(It.IsAny<string[]>())).Returns("Dott.");

        // Act
        var result = await _firstNameService.GenerateAsync(new FirstNameOptions { Gender = Gender.Male });

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GenerateAsync_WithFemaleGender_ShouldReturnFemaleName()
    {
        // Arrange
        _randomizerMock.Setup(r => r.Number(It.IsAny<int>(), It.IsAny<int>())).Returns(1); // 1 = Female
        _randomizerMock.Setup(r => r.ArrayElement(It.IsAny<string[]>())).Returns("Dott.ssa");

        // Act
        var result = await _firstNameService.GenerateAsync(new FirstNameOptions { Gender = Gender.Female });

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GenerateAsync_WithPrefixTrue_ShouldReturnNameWithPrefix()
    {
        // Arrange
        _randomizerMock.Setup(r => r.Number(It.IsAny<int>(), It.IsAny<int>())).Returns(0); // 0 = Male
        _randomizerMock.Setup(r => r.ArrayElement(It.IsAny<string[]>())).Returns("Dott.");

        // Act
        var result = await _firstNameService.GenerateAsync(new FirstNameOptions
        {
            Gender = Gender.Male,
            Prefix = true
        });

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Dott.", result);
    }

    [Fact]
    public void GetPrefix_WithMaleGender_ShouldReturnMalePrefix()
    {
        // Arrange
        _randomizerMock.Setup(r => r.ArrayElement(It.IsAny<string[]>())).Returns("Prof.");

        // Act
        var result = _firstNameService.GetPrefix(Gender.Male);

        // Assert
        Assert.Equal("Prof.", result);
    }

    [Fact]
    public void GetPrefix_WithFemaleGender_ShouldReturnFemalePrefix()
    {
        // Arrange
        _randomizerMock.Setup(r => r.ArrayElement(It.IsAny<string[]>())).Returns("Prof.ssa");

        // Act
        var result = _firstNameService.GetPrefix(Gender.Female);

        // Assert
        Assert.Equal("Prof.ssa", result);
    }

    [Fact]
    public void GetPrefix_WithNoGender_ShouldReturnNeutralPrefix()
    {
        // Arrange
        _randomizerMock.Setup(r => r.ArrayElement(It.IsAny<string[]>())).Returns("Ing.");

        // Act
        var result = _firstNameService.GetPrefix();

        // Assert
        Assert.Equal("Ing.", result);
    }

    [Fact]
    public void Generate_ShouldCallGenerateAsync()
    {
        // This test verifies that the sync method calls the async one

        // Arrange
        _randomizerMock.Setup(r => r.Number(It.IsAny<int>(), It.IsAny<int>())).Returns(0);
        _randomizerMock.Setup(r => r.ArrayElement(It.IsAny<string[]>())).Returns("Dott.");

        // Act & Assert (no exception means success)
        var result = _firstNameService.Generate();
        Assert.NotNull(result);
    }

    /// <summary>
    /// Helper method to set up embedded resource mocking by using reflection
    /// </summary>
    private void SetupEmbeddedResourceMocking()
    {
        string maleNamesJson = @"{
                ""items"": [
                    { ""value"": ""mario"", ""weight"": 100 },
                    { ""value"": ""giuseppe"", ""weight"": 80 },
                    { ""value"": ""antonio"", ""weight"": 60 }
                ]
            }";

        string femaleNamesJson = @"{
                ""items"": [
                    { ""value"": ""maria"", ""weight"": 100 },
                    { ""value"": ""anna"", ""weight"": 80 },
                    { ""value"": ""giovanna"", ""weight"": 60 }
                ]
            }";

        // Use reflection to get the private fields
        var dataLoadedField = typeof(FirstNameService).GetField("_dataLoaded",
            BindingFlags.NonPublic | BindingFlags.Instance);

        var maleSelectField = typeof(FirstNameService).GetField("_maleNamesSelector",
            BindingFlags.NonPublic | BindingFlags.Instance);

        var femaleSelectField = typeof(FirstNameService).GetField("_femaleNamesSelector",
            BindingFlags.NonPublic | BindingFlags.Instance);

        // Set _dataLoaded to true to avoid actual file loading
        dataLoadedField?.SetValue(_firstNameService, true);

        // Create and set the WeightedRandomSelector instances with our test data
        var maleData = System.Text.Json.JsonSerializer.Deserialize<WeightedData>(maleNamesJson);
        var femaleData = System.Text.Json.JsonSerializer.Deserialize<WeightedData>(femaleNamesJson);

        var maleSelector = new WeightedRandomSelector<string>(maleData!.Items);
        var femaleSelector = new WeightedRandomSelector<string>(femaleData!.Items);

        maleSelectField?.SetValue(_firstNameService, maleSelector);
        femaleSelectField?.SetValue(_firstNameService, femaleSelector);
    }
}