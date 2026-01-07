using System.Data;
using Application.Exceptions;
using Application.Ports.Repositories;
using Dapper;
using Domain.ExtensionModels;
using Domain.Product;

namespace Infrastructure.Repositories.Product;

/// <inheritdoc/>
internal class FavoriteProductsRepository : IFavoriteProductsRepository
{
    private readonly IDbConnection _dbConnection;
    
    public FavoriteProductsRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    
    /// <inheritdoc/>
    public async Task<IEnumerable<ResultProductModel>> GetFavoriteProductListAsync(Guid userId, int pageNumber, int pageSize)
    {
        var offset = (pageNumber - 1) * pageSize;
        var sql = $@"SELECT * 
                    FROM ""{nameof(ProductModel)}"" p
                    JOIN ""{nameof(FavoriteProductsModel)}"" fp 
                        ON p.""{nameof(ProductModel.Id)}"" = fp.""{nameof(FavoriteProductsModel.ProductId)}""
                    WHERE fp.""{nameof(FavoriteProductsModel.UserId)}"" = @UserId
                    ORDER BY p.""{nameof(ProductModel.Id)}""
                    OFFSET @Offset LIMIT @Limit
                    ";
        
        var result = await _dbConnection.QueryAsync<ResultProductModel>(sql, new
        {
            UserId = userId,
            Offset = offset,
            Limit = pageSize
        });
        return result;
    }

    /// <inheritdoc/>
    public async Task PostFavoriteProductAsync(Guid userId, Guid productId)
    {
        try
        {
            var sql = $@"INSERT INTO ""{nameof(FavoriteProductsModel)}"" VALUES (@Id, @ProductId, @UserId)";

            await _dbConnection.ExecuteAsync(sql, new
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                UserId = userId
            });
        }
        catch
        {
            throw new FavoriteProductException("An error occured while posting favorite product");
        }
    }

    /// <inheritdoc/>
    public async Task DeleteFavoriteProductAsync(Guid userId, Guid productId)
    {
        var sql = $@"DELETE FROM ""{nameof(FavoriteProductsModel)}""
                    WHERE ""{nameof(FavoriteProductsModel.UserId)}"" = @UserId
                    AND ""{nameof(FavoriteProductsModel.ProductId)}"" = @ProductId";
        
        await _dbConnection.ExecuteAsync(sql, new { UserId = userId, ProductId = productId });
    }

    /// <inheritdoc/>
    public async Task DeleteFavoriteProductListAsync(Guid userId, List<Guid> productIdList)
    {
        var sql = $@"DELETE FROM ""{nameof(FavoriteProductsModel)}""
                    WHERE ""{nameof(FavoriteProductsModel.UserId)}"" = @UserId
                    AND ""{nameof(FavoriteProductsModel.ProductId)}"" = ANY(@ProductId)";
        
        await _dbConnection.ExecuteAsync(sql, new
        {
            UserId = userId,
            ProductId = productIdList
        });
    }
}