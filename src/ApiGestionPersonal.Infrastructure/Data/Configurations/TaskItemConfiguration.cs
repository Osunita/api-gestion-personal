using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApiGestionPersonal.Domain.Entities;
using ApiGestionPersonal.Domain.Enums;

namespace ApiGestionPersonal.Infrastructure.Data.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("Tasks");
        
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Titulo)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(t => t.Contenido)
            .HasMaxLength(2000);
        
        builder.Property(t => t.Prioridad)
            .HasConversion<int>();
        
        builder.Property(t => t.Completada)
            .HasDefaultValue(false);
        
        // Relationship with Category
        builder.HasOne(t => t.Categoria)
            .WithMany(c => c.Tasks)
            .HasForeignKey(t => t.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Index for common queries
        builder.HasIndex(t => t.Completada);
        builder.HasIndex(t => t.Prioridad);
        builder.HasIndex(t => t.FechaVencimiento);
    }
}