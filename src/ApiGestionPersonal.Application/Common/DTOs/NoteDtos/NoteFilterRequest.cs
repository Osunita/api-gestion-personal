namespace ApiGestionPersonal.Application.Common.DTOs.NoteDtos;

public class NoteFilterRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public int? CategoriaId { get; set; }
}