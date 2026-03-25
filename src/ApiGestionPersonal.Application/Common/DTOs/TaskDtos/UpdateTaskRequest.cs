namespace ApiGestionPersonal.Application.Common.DTOs.TaskDtos;

public class UpdateTaskRequest
{
    public string Titulo { get; set; } = string.Empty;
    public string? Contenido { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public string Prioridad { get; set; } = "Media";
    public bool Completada { get; set; }
    public int? CategoriaId { get; set; }
}