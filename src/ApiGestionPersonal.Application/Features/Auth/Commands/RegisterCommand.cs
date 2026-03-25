using MediatR;
using ApiGestionPersonal.Application.Common.DTOs.AuthDtos;
using ApiGestionPersonal.Application.Common.Interfaces;
using ApiGestionPersonal.Domain.Entities;

namespace ApiGestionPersonal.Application.Features.Auth.Commands;

public class RegisterCommand : IRequest<AuthResponse>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(IUnitOfWork unitOfWork, IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Validate email format
        if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@"))
        {
            throw new ArgumentException("Invalid email format");
        }

        // Validate password strength
        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
        {
            throw new ArgumentException("Password must be at least 6 characters");
        }

        // Check if user already exists
        var existingUsers = await _unitOfWork.Users.GetAllAsync();
        if (existingUsers.Any(u => u.Email.ToLower() == request.Email.ToLower()))
        {
            throw new InvalidOperationException("A user with this email already exists");
        }

        // Hash password using BCrypt
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Create user
        var user = new User
        {
            Email = request.Email.ToLower(),
            PasswordHash = passwordHash,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

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