using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApiGestionPersonal.Domain.Entities;

namespace ApiGestionPersonal.Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Nombre)
            .IsRequired()
            .HasMaxLength(100);
        
        // Seed default category
        builder.HasData(
            new Category { Id = 1, Nombre = "General", FechaCreacion = DateTime.UtcNow }
        );
    }
}