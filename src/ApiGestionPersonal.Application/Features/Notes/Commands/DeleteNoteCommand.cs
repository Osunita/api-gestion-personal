using MediatR;
using ApiGestionPersonal.Application.Common.Interfaces;

namespace ApiGestionPersonal.Application.Features.Notes.Commands;

public class DeleteNoteCommand : IRequest<bool>
{
    public int Id { get; set; }
}

public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteNoteCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await _unitOfWork.Notes.GetByIdAsync(request.Id);
        if (note == null)
            return false;

        note.DeletedAt = DateTime.UtcNow;
        await _unitOfWork.Notes.UpdateAsync(note);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
}