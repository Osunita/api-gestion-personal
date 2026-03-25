namespace ApiGestionPersonal.Application.Common.DTOs.CategoryDtos;

public class CategoryResponse
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
}