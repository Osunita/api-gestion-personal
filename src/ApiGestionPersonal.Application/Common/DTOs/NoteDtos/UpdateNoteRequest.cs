namespace ApiGestionPersonal.Application.Common.DTOs.NoteDtos;

public class UpdateNoteRequest
{
    public string Titulo { get; set; } = string.Empty;
    public string? Contenido { get; set; }
    public string? Color { get; set; }
    public int? CategoriaId { get; set; }
}