namespace ApiGestionPersonal.Application.Common.DTOs.TaskDtos;

public class TaskResponse
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Contenido { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public string Prioridad { get; set; } = string.Empty;
    public bool Completada { get; set; }
    public int CategoriaId { get; set; }
    public string CategoriaNombre { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
}