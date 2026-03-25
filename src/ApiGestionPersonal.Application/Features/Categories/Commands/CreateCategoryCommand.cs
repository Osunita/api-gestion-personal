using MediatR;
using ApiGestionPersonal.Application.Common.DTOs.CategoryDtos;
using ApiGestionPersonal.Application.Common.Interfaces;
using ApiGestionPersonal.Domain.Entities;

namespace ApiGestionPersonal.Application.Features.Categories.Commands;

public class CreateCategoryCommand : IRequest<CategoryResponse>
{
    public string Nombre { get; set; } = string.Empty;
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var existing = await _unitOfWork.CategoryRepository.GetByNameAsync(request.Nombre);
        if (existing != null)
        {
            throw new InvalidOperationException("Category already exists");
        }

        var category = new Category
        {
            Nombre = request.Nombre,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return new CategoryResponse
        {
            Id = category.Id,
            Nombre = category.Nombre,
            FechaCreacion = category.FechaCreacion
        };
    }
}