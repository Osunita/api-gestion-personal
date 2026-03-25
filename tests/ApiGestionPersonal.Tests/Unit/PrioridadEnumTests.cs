using ApiGestionPersonal.Domain.Enums;

namespace ApiGestionPersonal.Tests.Unit;

public class PrioridadEnumTests
{
    [Fact]
    public void Prioridad_HasCorrectValues()
    {
        // Assert
        Assert.Equal(0, (int)Prioridad.Baja);
        Assert.Equal(1, (int)Prioridad.Media);
        Assert.Equal(2, (int)Prioridad.Alta);
    }

    [Theory]
    [InlineData("Baja", Prioridad.Baja)]
    [InlineData("Media", Prioridad.Media)]
    [InlineData("Alta", Prioridad.Alta)]
    public void Prioridad_ParseFromString_ReturnsCorrectEnum(string input, Prioridad expected)
    {
        // Act
        var result = Enum.Parse<Prioridad>(input, ignoreCase: true);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Prioridad_InvalidString_ThrowsException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Enum.Parse<Prioridad>("InvalidPriority"));
    }
}