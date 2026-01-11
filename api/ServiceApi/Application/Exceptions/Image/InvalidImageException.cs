using Application.Exceptions.Base;

namespace Application.Exceptions.Image;

/// <summary>
/// Невалидное изображение
/// </summary>
public class InvalidImageException : BaseException
{
    /// <inheritdoc />
    public InvalidImageException(string message) : base(message)
    {
    }
}