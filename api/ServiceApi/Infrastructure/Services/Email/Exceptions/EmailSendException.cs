using Application.Exceptions.Base;

namespace Infrastructure.Services.Email.Exceptions;

/// <summary>
/// Ошибка при отправке письма
/// </summary>
public class EmailSendException : BaseException
{
    public EmailSendException(string message) : base(message)
    {
    }

    public EmailSendException(string message, Exception inner) : base(message, inner)
    {
    }
}