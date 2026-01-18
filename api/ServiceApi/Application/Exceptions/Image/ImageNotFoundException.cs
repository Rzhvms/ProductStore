using Application.Exceptions.Base;

namespace Application.Exceptions.Image;

/// <summary>
/// Изображение не найдено
/// </summary>
public class ImageNotFoundException : BaseException
{
    /// <inheritdoc />
    public ImageNotFoundException(string message) : base(message)
    {
    }

}