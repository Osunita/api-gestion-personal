namespace ApiGestionPersonal.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
}