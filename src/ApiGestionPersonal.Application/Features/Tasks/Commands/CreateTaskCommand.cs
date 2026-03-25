using MediatR;
using ApiGestionPersonal.Application.Common.DTOs.TaskDtos;
using ApiGestionPersonal.Application.Common.Interfaces;

namespace ApiGestionPersonal.Application.Features.Tasks.Commands;

public class CreateTaskCommand : IRequest<TaskResponse>
{
    public string Titulo { get; set; } = string.Empty;
    public string? Contenido { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public string Prioridad { get; set; } = "Media";
}

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategorizationService _categorizationService;

    public CreateTaskCommandHandler(IUnitOfWork unitOfWork, ICategorizationService categorizationService)
    {
        _unitOfWork = unitOfWork;
        _categorizationService = categorizationService;
    }

    public async Task<TaskResponse> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        // Parse prioridad
        if (!Enum.TryParse<Domain.Enums.Prioridad>(request.Prioridad, true, out var prioridad))
        {
            prioridad = Domain.Enums.Prioridad.Media;
        }

        // Auto-categorize based on content
        var categoryName = _categorizationService.Categorize(request.Contenido ?? request.Titulo);
        var category = await _unitOfWork.CategoryRepository.GetByNameAsync(categoryName);
        
        if (category == null)
        {
            category = await _unitOfWork.CategoryRepository.GetByIdAsync(1); // Default "General"
        }

        var task = new Domain.Entities.TaskItem
        {
            Titulo = request.Titulo,
            Contenido = request.Contenido,
            FechaVencimiento = request.FechaVencimiento,
            Prioridad = prioridad,
            Completada = false,
            CategoriaId = category!.Id,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Tasks.AddAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return new TaskResponse
        {
            Id = task.Id,
            Titulo = task.Titulo,
            Contenido = task.Contenido,
            FechaVencimiento = task.FechaVencimiento,
            Prioridad = task.Prioridad.ToString(),
            Completada = task.Completada,
            CategoriaId = task.CategoriaId,
            CategoriaNombre = category.Nombre,
            FechaCreacion = task.FechaCreacion
        };
    }
}