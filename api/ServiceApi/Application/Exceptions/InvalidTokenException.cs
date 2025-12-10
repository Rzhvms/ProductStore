using Application.Exceptions.Base;

namespace Application.Exceptions;

/// <summary>
/// Кастомные ошибки при невалидном Refresh токене
/// </summary>
public class InvalidTokenException : BaseException
{
    public InvalidTokenException(string message) : base(message) { }
    public InvalidTokenException(string message, Exception innerException) : base(message, innerException) { }
}