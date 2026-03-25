namespace ApiGestionPersonal.Application.Common.DTOs.NoteDtos;

public class NoteResponse
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Contenido { get; set; }
    public string? Color { get; set; }
    public int CategoriaId { get; set; }
    public string CategoriaNombre { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
}