using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApiGestionPersonal.Domain.Entities;

namespace ApiGestionPersonal.Infrastructure.Data.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.ToTable("Notes");
        
        builder.HasKey(n => n.Id);
        
        builder.Property(n => n.Titulo)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(n => n.Contenido)
            .HasMaxLength(5000);
        
        builder.Property(n => n.Color)
            .HasMaxLength(7); // Hex color like #FFFFFF
        
        // Relationship with Category
        builder.HasOne(n => n.Categoria)
            .WithMany(c => c.Notes)
            .HasForeignKey(n => n.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Index
        builder.HasIndex(n => n.CategoriaId);
    }
}