using MediatR;
using ApiGestionPersonal.Application.Common.DTOs.TaskDtos;
using ApiGestionPersonal.Application.Common.Interfaces;

namespace ApiGestionPersonal.Application.Features.Tasks.Commands;

public class UpdateTaskCommand : IRequest<TaskResponse>
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Contenido { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public string Prioridad { get; set; } = "Media";
    public bool Completada { get; set; }
    public int? CategoriaId { get; set; }
}

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategorizationService _categorizationService;

    public UpdateTaskCommandHandler(IUnitOfWork unitOfWork, ICategorizationService categorizationService)
    {
        _unitOfWork = unitOfWork;
        _categorizationService = categorizationService;
    }

    public async Task<TaskResponse> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(request.Id);
        if (task == null)
            throw new KeyNotFoundException($"Task with id {request.Id} not found");

        if (!Enum.TryParse<Domain.Enums.Prioridad>(request.Prioridad, true, out var prioridad))
        {
            prioridad = Domain.Enums.Prioridad.Media;
        }

        // Re-categorize if content changed
        int categoryId = request.CategoriaId ?? task.CategoriaId;
        if (request.Contenido != null && request.Contenido != task.Contenido)
        {
            var categoryName = _categorizationService.Categorize(request.Contenido);
            var category = await _unitOfWork.CategoryRepository.GetByNameAsync(categoryName);
            if (category != null)
            {
                categoryId = category.Id;
            }
        }

        task.Titulo = request.Titulo;
        task.Contenido = request.Contenido;
        task.FechaVencimiento = request.FechaVencimiento;
        task.Prioridad = prioridad;
        task.Completada = request.Completada;
        task.CategoriaId = categoryId;

        await _unitOfWork.Tasks.UpdateAsync(task);
        await _unitOfWork.SaveChangesAsync();

        var categoryEntity = await _unitOfWork.Categories.GetByIdAsync(categoryId);

        return new TaskResponse
        {
            Id = task.Id,
            Titulo = task.Titulo,
            Contenido = task.Contenido,
            FechaVencimiento = task.FechaVencimiento,
            Prioridad = task.Prioridad.ToString(),
            Completada = task.Completada,
            CategoriaId = task.CategoriaId,
            CategoriaNombre = categoryEntity?.Nombre ?? "General",
            FechaCreacion = task.FechaCreacion
        };
    }
}