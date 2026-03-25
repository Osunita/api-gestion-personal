using ApiGestionPersonal.Domain.Common;

namespace ApiGestionPersonal.Domain.Entities;

public class Category : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    
    // Navigation properties
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    public ICollection<Note> Notes { get; set; } = new List<Note>();
}