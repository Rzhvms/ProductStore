using System.Data;
using Application.Ports.Repositories;
using Dapper;
using Domain.Product;

namespace Infrastructure.Repositories.Product;

/// <inheritdoc/>
internal class ProductReviewRepository : IProductReviewRepository
{
    private readonly IDbConnection _dbConnection;
    
    public ProductReviewRepository(IDbConnection dbConnection)
    { 
        _dbConnection = dbConnection;   
    }
    
    /// <inheritdoc/>
    public async Task<Guid> AddProductReviewAsync(Guid userId, Guid productId, int rating, string? message)
    {
        var sql = $@"INSERT INTO ""{nameof(ProductReviewModel)}"" 
            VALUES (@Id, @UserId, @ProductId, @Rating, @Message, @CreatedAt, @IsVisible)";
        
        var id = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;
        await _dbConnection.ExecuteAsync(sql, new
        {
            Id = id,
            UserId = userId,
            ProductId = productId,
            Rating = rating,
            Message = message,
            CreatedAt = createdAt,
            IsVisible = true
        });
        
        return id;
    }
    
    /// <inheritdoc/>
    public async Task<ProductReviewModel?> GetProductReviewAsync(Guid reviewId)
    {
        var sql = $@"SELECT * FROM ""{nameof(ProductReviewModel)}""
                    WHERE ""{nameof(ProductReviewModel.Id)}"" = @ReviewId";
        
        var review = await _dbConnection.QuerySingleOrDefaultAsync<ProductReviewModel>(sql, new
        {
            ReviewId = reviewId
        });

        return review;
    }
    
    /// <inheritdoc/>
    public async Task<List<ProductReviewModel>> GetProductReviewListAsync(Guid productId)
    {
        var sql = $@"SELECT * FROM ""{nameof(ProductReviewModel)}""
                    WHERE ""{nameof(ProductReviewModel.ProductId)}"" = @ProductId";
        
        var reviews = await _dbConnection.QueryAsync<ProductReviewModel>(sql, new
        {
            ProductId = productId
        });
        
        return reviews.AsList();
    }
    
    /// <inheritdoc/>
    public async Task DeleteProductReviewAsync(Guid reviewId)
    {
        var sql = $@"DELETE FROM ""{nameof(ProductReviewModel)}""
                    WHERE ""{nameof(ProductReviewModel.Id)}"" = @Id";

        await _dbConnection.ExecuteAsync(sql, new
        {
            Id = reviewId
        });
    }

    /// <inheritdoc/>
    public async Task ChangeIsVisibleAsync(Guid reviewId, bool isVisible)
    {
        var sql = $@"UPDATE ""{nameof(ProductReviewModel)}""
                    SET ""{nameof(ProductReviewModel.IsVisible)}"" = @IsVisible
                    WHERE ""{nameof(ProductReviewModel.Id)}"" = @Id";
        
        await _dbConnection.ExecuteAsync(sql, new
        {
            Id = reviewId, 
            IsVisible = isVisible
        });
    }

    /// <inheritdoc/>
    public async Task PatchProductReviewAsync(Guid reviewId, int? rating, string? message)
    {
        var updates = new List<string>();
        var parameters = new DynamicParameters();
        parameters.Add(nameof(ProductReviewModel.Id), reviewId);

        if (rating.HasValue)
        {
            updates.Add($@"""{nameof(ProductReviewModel.Rating)}"" = @Rating");
            parameters.Add(nameof(ProductReviewModel.Rating), rating.Value);
        }

        if (message != null)
        {
            updates.Add($@"""{nameof(ProductReviewModel.Message)}"" = @Message");
            parameters.Add(nameof(ProductReviewModel.Message), message);
        }

        if (updates.Count == 0)
        {
            return;
        }

        var sql = $@"UPDATE ""{nameof(ProductReviewModel)}""
                SET {string.Join(", ", updates)}
                WHERE ""{nameof(ProductReviewModel.Id)}"" = @Id";

        await _dbConnection.ExecuteAsync(sql, parameters);
    }
}