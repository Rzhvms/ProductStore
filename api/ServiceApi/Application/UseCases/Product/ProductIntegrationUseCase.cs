using System.Text.Encodings.Web;
using System.Text.Json;
using Application.Exceptions.Product;
using Application.Ports.Repositories;
using Application.UseCases.Product.Interfaces;
using Domain.ExtensionModels;
using Domain.Product;

namespace Application.UseCases.Product;

/// <inheritdoc/>
internal class ProductIntegrationUseCase : IProductIntegrationUseCase
{
    private readonly IProductRepository _repository;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
    
    public ProductIntegrationUseCase(IProductRepository repository)
    {
        _repository = repository;
    }
    
    /// <inheritdoc/>
    public async Task<ExportJsonFile> ExportProductByIdAsync(Guid id)
    {
        var product = await _repository.GetProductAsync(id);

        if (product is null)
        {
            throw new Exception("Product not found");
        }

        var json = JsonSerializer.Serialize(product, JsonOptions);
        var bytes = System.Text.Encoding.UTF8.GetBytes(json);

        return new ExportJsonFile(bytes, $"product_{id}.json");
    }
    
    /// <inheritdoc/>
    public async Task<ExportJsonFile> ExportProductListAsync()
    {
        var products = await _repository.GetAllProductsAsync();
        var json = JsonSerializer.Serialize(products, JsonOptions);
        var bytes = System.Text.Encoding.UTF8.GetBytes(json);

        return new ExportJsonFile(bytes, "products.json");
    }
    
    /// <inheritdoc/>
    public async Task ImportProductAsync(Stream jsonStream)
    {
        try
        {
            var product = await JsonSerializer.DeserializeAsync<ResultProductModel>(jsonStream, JsonOptions)
                          ?? throw new ProductIntegrationException("Невалидный JSON-файл");

            await Upsert(product);
        }
        catch (JsonException e)
        {
            throw new ProductIntegrationException("Невалидный JSON-файл. Необходимо импортировать один продукт.");
        }
    }

    /// <inheritdoc/>
    public async Task ImportProductListAsync(Stream jsonStream)
    {
        try
        {
            var products = await JsonSerializer.DeserializeAsync<List<ResultProductModel>>(jsonStream, JsonOptions)
                           ?? throw new ProductIntegrationException("Невалидный JSON-файл");

            foreach (var product in products)
            {
                await Upsert(product);
            }
        }
        catch (JsonException e)
        {
            throw new ProductIntegrationException("Невалидный JSON-файл. Необходимо импортировать список продуктов.");
        }
    }
    
    /// <summary>
    /// Добавление или изменение продукта
    /// </summary>
    private async Task Upsert(ResultProductModel product)
    {
        if (product.Id != Guid.Empty)
        {
            var existing = await _repository.GetProductAsync(product.Id);

            if (existing is not null)
            {
                await _repository.UpdateProductAsync(product.Id, product);
                return;
            }
        }

        await _repository.CreateProductAsync(product, product.Id == Guid.Empty ? null : product.Id);
    }
}