using Microsoft.EntityFrameworkCore;
using ApiGestionPersonal.Application.Common.Interfaces;
using ApiGestionPersonal.Domain.Entities;
using ApiGestionPersonal.Infrastructure.Data;

namespace ApiGestionPersonal.Infrastructure.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Category?> GetByNameAsync(string nombre)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.Nombre == nombre);
    }
}