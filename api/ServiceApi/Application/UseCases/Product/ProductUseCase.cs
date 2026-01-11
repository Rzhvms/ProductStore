using Application.Ports.Repositories;
using Application.UseCases.Product.Dto.Response;
using Application.UseCases.Product.Dto.Request;
using Application.UseCases.Product.Interfaces;
using Domain.ExtensionModels;

namespace Application.UseCases.Product;

/// <inheritdoc />
internal class ProductUseCase : IProductUseCase
{
    private readonly IProductRepository _repository;
    
    public ProductUseCase(IProductRepository repository)
    {
        _repository = repository;
    }
    
    /// <inheritdoc />
    public async Task<CreateProductResponse> CreateProductAsync(CreateProductRequest request)
    {
        var model = new ResultProductModel
        {
            Name = request.Name,
            ProviderId = request.ProviderId,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.Quantity,
            CategoryId = request.CategoryId,
            Characteristics =  request.Characteristics
        };
        
        var product = await _repository.CreateProductAsync(model);
        return new CreateProductResponse
        {
            ProductId = product
        };
    }

    /// <inheritdoc />
    public async Task<GetProductResponse> GetProductAsync(Guid id)
    {
        var response = await _repository.GetProductAsync(id);
        return new GetProductResponse
        {
            Id = response.Id,
            Name = response.Name,
            Description = response.Description,
            Price = response.Price,
            CategoryId = response.CategoryId,
            Characteristics = response.Characteristics
        };
    }
    
    /// <inheritdoc />
    public async Task<GetAdminProductResponse> GetAdminProductAsync(Guid id)
    {
        var response = await _repository.GetProductAsync(id);
        return new GetAdminProductResponse
        {
            Id = response.Id,
            Name = response.Name,
            Description = response.Description,
            ProviderId = response.ProviderId,
            Price = response.Price,
            Quantity = response.Quantity,
            CategoryId = response.CategoryId,
            Characteristics = response.Characteristics
        };
    }
    
    /// <inheritdoc />
    public async Task<GetProductListResponse> GetProductListAsync(int pageNumber, int pageSize)
    {
        var response = await _repository.GetProductListAsync(pageNumber, pageSize);
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
    
    /// <inheritdoc />
    public async Task<GetAdminProductListResponse> GetAdminProductListAsync(int pageNumber, int pageSize)
    {
        var response = await _repository.GetProductListAsync(pageNumber, pageSize);
        var productList = response.Select(p => new GetAdminProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            ProviderId = p.ProviderId,
            Price = p.Price,
            Quantity = p.Quantity,
            CategoryId = p.CategoryId,
            Characteristics = p.Characteristics
        }).ToList();
        
        return new GetAdminProductListResponse()
        {
            ProductList = productList
        };
    }
    
    /// <inheritdoc />
    public async Task<GetProductListResponse> GetProductListByCategoryIdAsync(Guid categoryId, int pageNumber, int pageSize)
    {
        var response = await _repository.GetProductListByCategoryIdAsync(categoryId, pageNumber, pageSize);
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

    public async Task<GetAdminProductListResponse> GetAdminProductListByCategoryIdAsync(Guid categoryId, int pageNumber,
        int pageSize)
    {
        var response = await _repository.GetProductListByCategoryIdAsync(categoryId, pageNumber, pageSize);
        var productList = response.Select(p => new GetAdminProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            ProviderId =  p.ProviderId,
            Price = p.Price,
            Quantity = p.Quantity,
            CategoryId = p.CategoryId,
            Characteristics = p.Characteristics
        }).ToList();
        
        return new GetAdminProductListResponse()
        {
            ProductList = productList
        };
    }
    
    /// <inheritdoc />
    public async Task<UpdateProductResponse> UpdateProductAsync(Guid id, UpdateProductRequest request)
    {
        var model = new ResultProductModel
        {
            Name = request.Name,
            ProviderId = request.ProviderId,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.Quantity,
            CategoryId = request.CategoryId,
            Characteristics =  request.Characteristics
        };
        await _repository.UpdateProductAsync(id, model); 
        
        return new UpdateProductResponse();
    }
    
    /// <inheritdoc />
    public async Task<DeleteProductResponse> DeleteProductAsync(Guid id)
    {
        await _repository.DeleteProductAsync(id);
        return new DeleteProductResponse();
    }
}