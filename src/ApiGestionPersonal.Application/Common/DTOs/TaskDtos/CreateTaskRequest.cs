namespace ApiGestionPersonal.Application.Common.DTOs.TaskDtos;

public class CreateTaskRequest
{
    public string Titulo { get; set; } = string.Empty;
    public string? Contenido { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public string Prioridad { get; set; } = "Media";
}