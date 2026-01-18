using Application.Ports.Repositories;
using Application.UseCases.Product.Dto.Request;
using Application.UseCases.Product.Dto.Response;

namespace Application.UseCases.Product;

/// <inheritdoc/>
internal class ProductReviewUseCase : IProductReviewUseCase
{
    private readonly IProductReviewRepository _productReviewRepository;

    public ProductReviewUseCase(IProductReviewRepository productReviewRepository)
    {
        _productReviewRepository = productReviewRepository;
    }
    
    /// <inheritdoc/>
    public async Task<AddProductReviewResponse> AddProductReviewAsync(Guid userId, AddProductReviewRequest request)
    {
        var id = await _productReviewRepository.AddProductReviewAsync(userId, request.ProductId, request.Rating, request.Message);
        return new AddProductReviewResponse
        {
            ReviewId = id
        };
    }
    
    /// <inheritdoc/>
    public async Task<GetProductReviewResponse> GetProductReviewAsync(Guid reviewId)
    {
        var review = await _productReviewRepository.GetProductReviewAsync(reviewId);
        return new GetProductReviewResponse
        {
            Id = review.Id,
            UserId = review.UserId,
            ProductId = review.ProductId,
            Rating = review.Rating,
            Message = review.Message,
            CreatedAt = review.CreatedAt,
            IsVisible = review.IsVisible
        };
    }
    
    /// <inheritdoc/>
    public async Task<GetProductReviewListResponse> GetProductReviewListAsync(Guid productId)
    {
        var reviews = await _productReviewRepository.GetProductReviewListAsync(productId);

        var response = reviews.Select(x => new GetProductReviewResponse()
        {
            Id = x.Id,
            UserId = x.UserId,
            ProductId = x.ProductId,
            Rating = x.Rating,
            Message = x.Message,
            CreatedAt = x.CreatedAt,
            IsVisible = x.IsVisible
        });

        return new GetProductReviewListResponse
        {
            ProductReviewList = response.ToList()
        };
    }
    
    /// <inheritdoc/>
    public async Task DeleteProductReviewAsync(Guid reviewId)
    {
        await _productReviewRepository.DeleteProductReviewAsync(reviewId);
    }

    /// <inheritdoc/>
    public async Task ChangeIsVisibleAsync(Guid reviewId, ChangeIsVisibleRequest request)
    {
        await _productReviewRepository.ChangeIsVisibleAsync(reviewId, request.IsVisible);
    }

    /// <inheritdoc/>
    public async Task PatchProductReviewAsync(Guid reviewId, PatchProductReviewRequest request)
    {
        await _productReviewRepository.PatchProductReviewAsync(reviewId, request.Rating, request.Message);
    }

    /// <inheritdoc/>
    public async Task<GetAverageRatingResponse> GetAverageRatingAsync(Guid productId)
    {
        var rating = await _productReviewRepository.GetAverageRatingAsync(productId);
        return new GetAverageRatingResponse
        {
            Rating = rating
        };
    }
}