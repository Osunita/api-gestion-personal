namespace ApiGestionPersonal.Application.Common.DTOs;

public class SuccessResponse<T>
{
    public T Data { get; set; } = default!;
    public string Message { get; set; } = string.Empty;
}