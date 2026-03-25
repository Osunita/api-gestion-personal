using Microsoft.EntityFrameworkCore.Storage;
using ApiGestionPersonal.Application.Common.Interfaces;
using ApiGestionPersonal.Domain.Entities;
using ApiGestionPersonal.Infrastructure.Data;

namespace ApiGestionPersonal.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    private IRepository<TaskItem>? _tasks;
    private IRepository<Note>? _notes;
    private IRepository<Category>? _categories;
    private IRepository<User>? _users;
    private ICategoryRepository? _categoryRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IRepository<TaskItem> Tasks => 
        _tasks ??= new Repositories.Repository<TaskItem>(_context);

    public IRepository<Note> Notes => 
        _notes ??= new Repositories.Repository<Note>(_context);

    public IRepository<Category> Categories => 
        _categories ??= new Repositories.Repository<Category>(_context);

    public IRepository<User> Users => 
        _users ??= new Repositories.Repository<User>(_context);

    public ICategoryRepository CategoryRepository => 
        _categoryRepository ??= new Repositories.CategoryRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}