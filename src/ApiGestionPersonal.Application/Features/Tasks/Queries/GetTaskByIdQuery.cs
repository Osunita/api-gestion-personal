using MediatR;
using ApiGestionPersonal.Application.Common.DTOs.TaskDtos;
using ApiGestionPersonal.Application.Common.Interfaces;

namespace ApiGestionPersonal.Application.Features.Tasks.Queries;

public class GetTaskByIdQuery : IRequest<TaskResponse>
{
    public int Id { get; set; }
}

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTaskByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TaskResponse> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(request.Id);
        if (task == null)
            throw new KeyNotFoundException($"Task with id {request.Id} not found");

        var category = await _unitOfWork.Categories.GetByIdAsync(task.CategoriaId);

        return new TaskResponse
        {
            Id = task.Id,
            Titulo = task.Titulo,
            Contenido = task.Contenido,
            FechaVencimiento = task.FechaVencimiento,
            Prioridad = task.Prioridad.ToString(),
            Completada = task.Completada,
            CategoriaId = task.CategoriaId,
            CategoriaNombre = category?.Nombre ?? "General",
            FechaCreacion = task.FechaCreacion
        };
    }
}