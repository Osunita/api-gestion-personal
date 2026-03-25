using ApiGestionPersonal.Domain.Entities;

namespace ApiGestionPersonal.Application.Common.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByNameAsync(string nombre);
}