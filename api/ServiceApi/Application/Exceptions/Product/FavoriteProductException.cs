using Application.Exceptions.Base;

namespace Application.Exceptions.Product;

/// <summary>
/// Ошибка связанная с избранными товарами
/// </summary>
public class FavoriteProductException : BaseException
{
    public FavoriteProductException(string message) : base(message) { }
}