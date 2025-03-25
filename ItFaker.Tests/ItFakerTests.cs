using ItFaker.Common;
using ItFaker.Models;
using ItFaker.Services;
using Moq;

namespace ItFaker.Tests;

public class ItFakerTests
{
    [Fact]
    public void Constructor_WithoutSeed_ShouldCreateValidInstance()
    {
        // Act
        var faker = new ItFaker();

        // Assert
        Assert.NotNull(faker);
        Assert.NotNull(faker.FirstNameService);
    }

    [Fact]
    public void Constructor_WithCustomRandomizer_ShouldUseProvidedRandomizer()
    {
        // Arrange
        var randomizerMock = new Mock<IRandomizer>();

        // Act
        var faker = new ItFaker(randomizerMock.Object);

        // Assert
        Assert.NotNull(faker);
        Assert.NotNull(faker.FirstNameService);
    }

    [Fact]
    public async Task FirstNameService_ShouldGenerateConsistentNames()
    {
        // Arrange
        var firstNameServiceMock = new Mock<IFirstNameService>();
        firstNameServiceMock.Setup(m => m.GenerateAsync(It.IsAny<FirstNameOptions>()))
            .ReturnsAsync("Mario");

        // Create a test helper to replace the real service with our mock
        var faker = CreateFakerWithMockedFirstNameService(firstNameServiceMock.Object);

        // Act
        var name = await faker.FirstNameService.Value.GenerateAsync(new FirstNameOptions { Gender = Gender.Male });

        // Assert
        Assert.Equal("Mario", name);
        firstNameServiceMock.Verify(m => m.GenerateAsync(It.IsAny<FirstNameOptions>()), Times.Once);
    }

    [Fact]
    public void FirstNameService_LazyLoading_ShouldWorkCorrectly()
    {
        // Arrange
        var firstNameServiceMock = new Mock<IFirstNameService>();
        var faker = CreateFakerWithMockedFirstNameService(firstNameServiceMock.Object);

        // The service shouldn't be accessed yet
        firstNameServiceMock.Verify(f => f.Generate(It.IsAny<FirstNameOptions>()), Times.Never);
        firstNameServiceMock.Verify(f => f.GenerateAsync(It.IsAny<FirstNameOptions>()), Times.Never);

        // Act - Access the lazy-loaded service
        var service = faker.FirstNameService.Value;

        Assert.NotNull(service);
        firstNameServiceMock.Verify(f => f.Generate(It.IsAny<FirstNameOptions>()), Times.Never);
        firstNameServiceMock.Verify(f => f.GenerateAsync(It.IsAny<FirstNameOptions>()), Times.Never);
    }

    /// <summary>
    /// Creates an ItFaker instance with a mocked FirstNameService
    /// </summary>
    private ItFaker CreateFakerWithMockedFirstNameService(IFirstNameService mockedService)
    {
        // Create a mock randomizer
        var randomizerMock = new Mock<IRandomizer>();

        // Create the ItFaker instance with the mock randomizer
        var faker = new ItFaker(randomizerMock.Object);

        // Use reflection to replace the service directly without Lazy wrapping
        var field = typeof(ItFaker).GetField("_firstNameService",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Set the field value directly with the mocked service
        field?.SetValue(faker, mockedService);

        return faker;
    }
}