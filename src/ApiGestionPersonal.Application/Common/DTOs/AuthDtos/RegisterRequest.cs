namespace ApiGestionPersonal.Application.Common.DTOs.AuthDtos;

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}