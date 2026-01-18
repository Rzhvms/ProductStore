using System.Data;
using Application.Exceptions.Product;
using Application.Ports.Repositories;
using Dapper;
using Domain.ExtensionModels;
using Domain.Product;

namespace Infrastructure.Repositories.Product;

/// <inheritdoc />
internal class ProductRepository : IProductRepository
{
    private readonly IDbConnection _connection;
    
    public ProductRepository(IDbConnection connection)
    {
        _connection = connection;
    }
    
    /// <inheritdoc />
    public async Task<Guid> CreateProductAsync(ResultProductModel model)
    {
        using var transaction = _connection.BeginTransaction();
        try
        {
            // Создаем новый продукт в базе
            var productId = Guid.NewGuid();
            var sql = $@"INSERT INTO ""{nameof(ProductModel)}"" 
                            VALUES (@Id, @Name, @ProviderId, @CategoryId, @Description, @Price, @Quantity, @Characteristics)";
            
            await _connection.ExecuteAsync(sql, new
            {
                Id = productId,
                Name = model.Name,
                ProviderId = model.ProviderId,
                CategoryId = model.CategoryId,
                Description = model.Description,
                Price = model.Price,
                Quantity = model.Quantity,
                Characteristics = model.Characteristics

            }, transaction);

            transaction.Commit();
            return productId;
        }
        catch
        {
            transaction.Rollback();
            throw new CreateProductException("Ошибка при создании продукта");
        }
    }

    /// <inheritdoc />
    public async Task<ResultProductModel?> GetProductAsync(Guid id)
    {
        var sqlGetProduct = $@"SELECT * FROM ""{nameof(ProductModel)}""
                                WHERE ""{nameof(ProductModel.Id)}"" = @Id";
        
        var product = await _connection.QueryFirstOrDefaultAsync<ResultProductModel>(sqlGetProduct, new { Id = id });
        return product;
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<ResultProductModel>> GetProductListAsync(int pageNumber, int pageSize)
    {
        var offset = (pageNumber - 1) * pageSize;
        var sqlGetProduct = $@"SELECT * FROM ""{nameof(ProductModel)}""
                                ORDER BY ""{nameof(ProductModel.Id)}""
                                OFFSET @Offset LIMIT @PageSize";
        var products = await _connection.QueryAsync<ResultProductModel>(sqlGetProduct,  new
        {
            Offset = offset, 
            PageSize = pageSize
        });
        return products;
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<ResultProductModel>> GetProductListByCategoryIdAsync(Guid categoryId, int pageNumber, int pageSize)
    {
        var offset = (pageNumber - 1) * pageSize;
        var sqlGetProduct = $@"SELECT * FROM ""{nameof(ProductModel)}""
                                WHERE ""{nameof(ProductModel.CategoryId)}"" = @CategoryId
                                ORDER BY ""{nameof(ProductModel.Id)}""
                                OFFSET @Offset LIMIT @PageSize";
        var products = await _connection.QueryAsync<ResultProductModel>(sqlGetProduct,  new
        {
            CategoryId = categoryId,
            Offset = offset, 
            PageSize = pageSize
        });
        return products;
    }
    
    /// <inheritdoc />
    public async Task UpdateProductAsync(Guid id, ResultProductModel model)
    {
        var sql = $@"UPDATE ""{nameof(ProductModel)}""
                    SET ""{nameof(ProductModel.Name)}"" = @Name,
                        ""{nameof(ProductModel.Description)}"" = @Description,
                        ""{nameof(ProductModel.ProviderId)}"" = @ProviderId,
                        ""{nameof(ProductModel.CategoryId)}"" = @CategoryId,
                        ""{nameof(ProductModel.Price)}"" = @Price,
                        ""{nameof(ProductModel.Quantity)}"" = @Quantity,
                        ""{nameof(ProductModel.Characteristics)}"" = @Characteristics
                    WHERE ""{nameof(ProductModel.Id)}"" = @Id";
        await _connection.ExecuteAsync(sql, new
        {
            Id = id,
            Name = model.Name,
            Description = model.Description,
            ProviderId = model.ProviderId,
            CategoryId = model.CategoryId,
            Price = model.Price,
            Quantity = model.Quantity,
            Characteristics = model.Characteristics
        });
    }
    
    /// <inheritdoc />
    public async Task DeleteProductAsync(Guid id)
    {
        var sql = $@"DELETE FROM ""{nameof(ProductModel)}""
                        WHERE ""{nameof(ProductModel.Id)}"" = @Id";
        await _connection.ExecuteAsync(sql, new { Id = id });
    }
}