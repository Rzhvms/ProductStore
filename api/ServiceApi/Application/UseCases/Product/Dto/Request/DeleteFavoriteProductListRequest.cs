using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.Product.Dto.Request;

/// <summary>
/// Входная модель удаления списка товаров из избранного
/// </summary>
public record DeleteFavoriteProductListRequest
{
    /// <summary>
    /// Список идентификаторов удаляемых продуктов
    /// </summary>
    [Required]
    [JsonPropertyName("productIdList")]
    public required List<Guid> ProductIdList { get; set; }
}