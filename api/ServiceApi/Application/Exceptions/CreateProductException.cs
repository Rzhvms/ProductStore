using Application.Exceptions.Base;

namespace Application.Exceptions;

/// <summary>
/// Кастомная ошибка при создании продукта
/// </summary>
public class CreateProductException : BaseException
{
    public CreateProductException(string message) : base(message) { }
}