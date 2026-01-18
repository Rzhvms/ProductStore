using Application.Exceptions.Category;
using Application.Ports.Repositories;
using Application.UseCases.Category.Dto.Request;
using Application.UseCases.Category.Dto.Response;
using Domain.Product;

namespace Application.UseCases.Category;

/// <inheritdoc/>
internal class CategoryUseCase : ICategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    /// <inheritdoc/>
    public async Task<CreateCategoryResponse> CreateCategoryAsync(CreateCategoryRequest request)
    {
        var categoryId =  await _categoryRepository.CreateCategoryAsync(request.Name, request.ParentId);
        return new CreateCategoryResponse(){ Id = categoryId };
    }

    /// <inheritdoc/>
    public async Task<GetCategoryResponse> GetCategoryAsync(Guid id)
    {
        var category = await _categoryRepository.GetCategoryAsync(id);
        if (category == null)
        {
            throw new InvalidCategoryException(nameof(category));
        }
            
        return new GetCategoryResponse()
        {
            CategoryId = category.Id,
            CategoryName = category.Name,
            ParentCategoryId = category.ParentId
        };
    }

    /// <inheritdoc/>
    public async Task<List<GetCategoryResponse>> GetCategoryListAsync()
    {
        var categoryList = await _categoryRepository.GetCategoryListAsync();
        var categoryResponse = categoryList.Select(x => new GetCategoryResponse()
        {
            CategoryId = x.Id,
            CategoryName = x.Name,
            ParentCategoryId = x.ParentId
        });
        return categoryResponse.ToList();
    }

    /// <inheritdoc/>
    public async Task UpdateCategoryAsync(UpdateCategoryRequest request, Guid id)
    {
        var model = new CategoryModel()
        {
            Id = id,
            Name = request.Name,
            ParentId = request.ParentId
        };
        
        await _categoryRepository.UpdateCategoryAsync(model);
    }

    /// <inheritdoc/>
    public async Task DeleteCategoryAsync(Guid id)
    {
        await _categoryRepository.DeleteCategoryAsync(id);
    }
}
