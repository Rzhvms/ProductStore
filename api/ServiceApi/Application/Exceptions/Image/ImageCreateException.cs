using Application.Exceptions.Base;

namespace Application.Exceptions.Image;

/// <summary>
/// Ошибка при создании изображения
/// </summary>
public class ImageCreateException : BaseException
{
    public ImageCreateException(string message) : base(message)
    {
    }

}