using Application.Ports.Repositories;
using Application.UseCases.Product.Dto.Request;
using Application.UseCases.Product.Dto.Response;

namespace Application.UseCases.Product;

/// <inheritdoc/>
internal class FavoriteProductsUseCase : IFavoriteProductsUseCase
{
    private readonly IFavoriteProductsRepository _favoriteProductsRepository;
    
    public FavoriteProductsUseCase(IFavoriteProductsRepository favoriteProductsRepository)
    {
        _favoriteProductsRepository = favoriteProductsRepository;
    }
    
    /// <inheritdoc/>
    public async Task<GetProductListResponse> GetFavoriteProductListAsync(Guid userId)
    {
        var response = await _favoriteProductsRepository.GetFavoriteProductListAsync(userId);
        var productList = response.Select(p => new GetProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            CategoryId = p.CategoryId,
            Characteristics = p.Characteristics
        }).ToList();
        
        return new GetProductListResponse()
        {
            ProductList = productList
        };
    }

    /// <inheritdoc/>
    public async Task PostFavoriteProductAsync(Guid userId, Guid productId)
    {
        await _favoriteProductsRepository.PostFavoriteProductAsync(userId, productId);
    }

    /// <inheritdoc/>
    public async Task DeleteFavoriteProductAsync(Guid userId, Guid productId)
    {
        await _favoriteProductsRepository.DeleteFavoriteProductAsync(userId, productId);
    }

    /// <inheritdoc/>
    public async Task DeleteFavoriteProductListAsync(Guid userId, DeleteFavoriteProductListRequest request)
    {
        await _favoriteProductsRepository.DeleteFavoriteProductListAsync(userId, request.ProductIdList);
    }
}   
