using Application.Exceptions.Base;

namespace Application.Exceptions.User;

/// <summary>
/// Общая ошибка, связанная с валидацией и обработкой пользователя
/// </summary>
public class InvalidUserException : BaseException
{
    public InvalidUserException(string message) : base(message) { }
    public InvalidUserException(string message, Exception innerException) : base(message, innerException) { }
}