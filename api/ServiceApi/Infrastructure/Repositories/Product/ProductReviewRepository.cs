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
    public async Task GetProductReviewListAsync()
    {
        
    }
    
    /// <inheritdoc/>
    public async Task DeleteProductReviewAsync()
    {
        
    }
}