using System.Text.Json;
using Application.Exceptions;
using Application.Ports.Repositories;
using Application.UseCases.Cart.Dto.Response;
using StackExchange.Redis;

namespace Infrastructure.Repositories.Cart;

/// <inheritdoc/>
public class RedisCartRepository : ICartRepository
{
    private readonly IDatabase _database;
    private readonly IProductRepository _productRepository;
    private const int CartTtlHours = 48;
    private readonly JsonSerializerOptions _jsonOptions;

    public RedisCartRepository(IConnectionMultiplexer redis, IProductRepository productRepository)
    {
        _database = redis.GetDatabase();
        _productRepository = productRepository;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
    }
    
    /// <inheritdoc/>
    public async Task<GetCartResponse> GetCartAsync(Guid userId)
    {
        var key = GetKey(userId);
        var value = await _database.StringGetAsync(key);
        if (!value.HasValue)
            return new GetCartResponse();

        return JsonSerializer.Deserialize<GetCartResponse>(value!, _jsonOptions) ?? new GetCartResponse();
    }

    /// <inheritdoc/>
    public async Task AddOrUpdateItemAsync(Guid userId, CartItemDto item)
    {
        // Проверяем продукт через репозиторий
        var product = await _productRepository.GetProductAsync(item.ProductId);
        if (product is null)
            throw new ProductNotFoundException($"Продукт с Id {item.ProductId} не найден");

        // Если quantity == 0 - удаляем товар из корзины
        if (item.Quantity == 0)
        {
            await RemoveItemAsync(userId, item.ProductId);
            return;
        }
        
        if (item.Quantity <= 0)
            throw new InvalidCartException("Количество должно быть больше 0");

        // Запрещаем поставить в корзину больше товаров, чем есть в БД
        if (item.Quantity > product.Quantity)
            throw new InvalidCartException("Превышено максимальное количество товаров в наличии");
        
        var cart = await GetCartAsync(userId);

        var existing = cart.Items.FirstOrDefault(x => x.ProductId == item.ProductId);
        if (existing != null)
        {
            existing.Quantity = item.Quantity;
            existing.Price = product.Price; // обновляем цену на актуальную
        }
        else
        {
            cart.Items.Add(new CartItemDto
            {
                ProductId = product.Id,
                Name = product.Name,
                Quantity = item.Quantity,
                Price = product.Price
            });
        }

        var key = GetKey(userId);
        await _database.StringSetAsync(key, JsonSerializer.Serialize(cart, _jsonOptions), TimeSpan.FromHours(CartTtlHours));
    }

    /// <inheritdoc/>
    public async Task RemoveItemAsync(Guid userId, Guid productId)
    {
        var cart = await GetCartAsync(userId);
        cart.Items.RemoveAll(x => x.ProductId == productId);

        var key = GetKey(userId);
        await _database.StringSetAsync(key, JsonSerializer.Serialize(cart, _jsonOptions), TimeSpan.FromHours(CartTtlHours));
    }

    /// <inheritdoc/>
    public async Task ClearCartAsync(Guid userId)
    {
        var key = GetKey(userId);
        await _database.KeyDeleteAsync(key);
    }

    /// <summary>
    /// Получить ключ
    /// </summary>
    private static string GetKey(Guid userId) => $"cart:{userId}";
}
