using ApiGestionPersonal.Domain.Common;
using ApiGestionPersonal.Domain.Enums;

namespace ApiGestionPersonal.Domain.Entities;

public class TaskItem : BaseEntity
{
    public string Titulo { get; set; } = string.Empty;
    public string? Contenido { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public Prioridad Prioridad { get; set; } = Prioridad.Media;
    public bool Completada { get; set; } = false;
    public int CategoriaId { get; set; }
    
    // Navigation property
    public Category Categoria { get; set; } = null!;
}