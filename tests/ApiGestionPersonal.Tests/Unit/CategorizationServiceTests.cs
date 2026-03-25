using ApiGestionPersonal.Infrastructure.Services;

namespace ApiGestionPersonal.Tests.Unit;

public class CategorizationServiceTests
{
    private readonly KeywordCategorizationService _service;

    public CategorizationServiceTests()
    {
        _service = new KeywordCategorizationService();
    }

    [Fact]
    public void Categorize_WithTrabajoKeywords_ReturnsTrabajo()
    {
        // Arrange
        var content = "Tengo una reunión a las 3pm";

        // Act
        var result = _service.Categorize(content);

        // Assert
        Assert.Equal("trabajo", result);
    }

    [Fact]
    public void Categorize_WithComprasKeywords_ReturnsCompras()
    {
        // Arrange
        var content = "Necesito buy groceries from the store";

        // Act
        var result = _service.Categorize(content);

        // Assert
        Assert.Equal("compras", result);
    }

    [Fact]
    public void Categorize_WithUrgenteKeywords_ReturnsPrioridadAlta()
    {
        // Arrange
        var content = "This is an urgent task";

        // Act
        var result = _service.Categorize(content);

        // Assert
        Assert.Equal("prioridad-alta", result);
    }

    [Fact]
    public void Categorize_WithComunicacionKeywords_ReturnsComunicacion()
    {
        // Arrange
        var content = "I need to call my mom today";

        // Act
        var result = _service.Categorize(content);

        // Assert
        Assert.Equal("comunicación", result);
    }

    [Fact]
    public void Categorize_WithPersonalKeywords_ReturnsPersonal()
    {
        // Arrange
        var content = "Family gathering at home";

        // Act
        var result = _service.Categorize(content);

        // Assert
        Assert.Equal("personal", result);
    }

    [Fact]
    public void Categorize_WithNoKeywords_ReturnsGeneral()
    {
        // Arrange
        var content = "Random text with no specific keywords";

        // Act
        var result = _service.Categorize(content);

        // Assert
        Assert.Equal("General", result);
    }

    [Fact]
    public void Categorize_WithEmptyString_ReturnsGeneral()
    {
        // Arrange
        var content = "";

        // Act
        var result = _service.Categorize(content);

        // Assert
        Assert.Equal("General", result);
    }

    [Fact]
    public void Categorize_WithNull_ReturnsGeneral()
    {
        // Arrange
        string? content = null;

        // Act
        var result = _service.Categorize(content!);

        // Assert
        Assert.Equal("General", result);
    }

    [Fact]
    public void Categorize_CaseInsensitive_ReturnsCorrectCategory()
    {
        // Arrange
        var content = "MEETING with team tomorrow"; // uppercase

        // Act
        var result = _service.Categorize(content);

        // Assert
        Assert.Equal("trabajo", result);
    }

    [Theory]
    [InlineData("reunión", "trabajo")]
    [InlineData("comprar", "compras")]
    [InlineData("urgent", "prioridad-alta")]
    [InlineData("call", "comunicación")]
    [InlineData("family", "personal")]
    public void Categorize_VariousKeywords_ReturnsExpectedCategory(string keyword, string expectedCategory)
    {
        // Arrange
        var content = $"This contains the keyword {keyword} in text";

        // Act
        var result = _service.Categorize(content);

        // Assert
        Assert.Equal(expectedCategory, result);
    }
}