using Application.Exceptions.Base;

namespace Application.Exceptions;

/// <summary>
/// Продукт не найден
/// </summary>
public class ProductNotFoundException : BaseException
{
    public ProductNotFoundException(string message) : base(message) { }
}