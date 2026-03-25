using MediatR;
using ApiGestionPersonal.Application.Common.DTOs.CategoryDtos;
using ApiGestionPersonal.Application.Common.Interfaces;

namespace ApiGestionPersonal.Application.Features.Categories.Queries;

public class GetCategoriesQuery : IRequest<IEnumerable<CategoryResponse>>
{
}

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoriesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CategoryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();

        return categories.Select(c => new CategoryResponse
        {
            Id = c.Id,
            Nombre = c.Nombre,
            FechaCreacion = c.FechaCreacion
        });
    }
}