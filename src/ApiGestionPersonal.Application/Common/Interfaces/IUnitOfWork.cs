namespace ApiGestionPersonal.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Domain.Entities.TaskItem> Tasks { get; }
    IRepository<Note> Notes { get; }
    IRepository<Category> Categories { get; }
    IRepository<User> Users { get; }
    ICategoryRepository CategoryRepository { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}