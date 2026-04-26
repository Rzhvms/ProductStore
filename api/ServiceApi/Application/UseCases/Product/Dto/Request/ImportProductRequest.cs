using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Product.Dto.Request;

/// <summary>
/// Входная dto-модель импорта файла
/// </summary>
public record ImportProductRequest
{
    public IFormFile File { get; set; } = null!;
}