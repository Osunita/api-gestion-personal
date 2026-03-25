namespace ApiGestionPersonal.Application.Common.DTOs.NoteDtos;

public class CreateNoteRequest
{
    public string Titulo { get; set; } = string.Empty;
    public string? Contenido { get; set; }
    public string? Color { get; set; }
}