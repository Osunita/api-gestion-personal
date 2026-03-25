using MediatR;
using ApiGestionPersonal.Application.Common.DTOs.AuthDtos;
using ApiGestionPersonal.Application.Common.Interfaces;

namespace ApiGestionPersonal.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<AuthResponse>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(IUnitOfWork unitOfWork, IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Find user by email
        var users = await _unitOfWork.Users.FindAsync(u => u.Email.ToLower() == request.Email.ToLower());
        var user = users.FirstOrDefault();

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        // Verify password with BCrypt
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        // Generate JWT token
        var token = _jwtService.GenerateToken(user.Id, user.Email);
        var expiresAt = DateTime.UtcNow.AddHours(1);

        return new AuthResponse
        {
            Token = token,
            Email = user.Email,
            ExpiresAt = expiresAt
        };
    }
}