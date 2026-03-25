using ApiGestionPersonal.Domain.Common;

namespace ApiGestionPersonal.Domain.Entities;

public class Note : BaseEntity
{
    public string Titulo { get; set; } = string.Empty;
    public string? Contenido { get; set; }
    public string? Color { get; set; }
    public int CategoriaId { get; set; }
    
    // Navigation property
    public Category Categoria { get; set; } = null!;
}