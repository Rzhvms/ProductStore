using Application.Exceptions.Base;

namespace Application.Exceptions.Category;

/// <summary>
/// Ошибка при обработке некорректной или отсутствующей категории
/// </summary>
public class InvalidCategoryException : BaseException
{
    public InvalidCategoryException(string message) : base(message) { }
}