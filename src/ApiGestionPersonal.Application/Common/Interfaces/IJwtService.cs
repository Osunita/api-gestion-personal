namespace ApiGestionPersonal.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(int userId, string email);
    int? ValidateToken(string token);
}