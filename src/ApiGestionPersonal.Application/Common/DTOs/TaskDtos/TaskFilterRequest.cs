namespace ApiGestionPersonal.Application.Common.DTOs.TaskDtos;

public class TaskFilterRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public string? Prioridad { get; set; }
    public bool? Completada { get; set; }
}