using System.Data;
using Application.Ports.Repositories;
using Dapper;
using Domain.Product;

namespace Infrastructure.Repositories.Category;

/// <inheritdoc/>
public class CategoryRepository : ICategoryRepository
{
    private readonly IDbConnection _connection;

    public CategoryRepository(IDbConnection connection)
    {
        _connection = connection;
    }
    
    /// <inheritdoc/>
    public async Task<Guid> CreateCategoryAsync(string categoryName, Guid? parentId)
    {
        var categoryId = Guid.NewGuid();
        var sql = $@"INSERT INTO ""{nameof(CategoryModel)}"" VALUES (@Id, @Name, @ParentId)";
        await _connection.ExecuteAsync(sql, new { Id = categoryId, Name = categoryName, ParentId = parentId });
        return categoryId;
    }

    /// <inheritdoc/>
    public async Task<CategoryModel?> GetCategoryAsync(Guid categoryId)
    {
        var sql = $@"SELECT * FROM ""{nameof(CategoryModel)}"" WHERE ""{nameof(CategoryModel.Id)}"" = @Id";
        var category = await _connection.QueryFirstOrDefaultAsync<CategoryModel>(sql, new { Id = categoryId });
        return category;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CategoryModel?>> GetCategoryListAsync()
    {
        var sql = $@"SELECT * FROM ""{nameof(CategoryModel)}""";
        var categoryList = await _connection.QueryAsync<CategoryModel>(sql);
        return categoryList;
    }

    /// <inheritdoc/>
    public async Task UpdateCategoryAsync(CategoryModel category)
    {
        var sql = $@"UPDATE ""{nameof(CategoryModel)}""
                    SET ""{nameof(CategoryModel.Name)}"" = @Name,
                        ""{nameof(CategoryModel.ParentId)}"" = @ParentId
                    WHERE ""{nameof(CategoryModel.Id)}"" = @Id";
        
        await _connection.ExecuteAsync(sql, new { category.Name, category.ParentId, category.Id });
    }

    /// <inheritdoc/>
    public async Task DeleteCategoryAsync(Guid id)
    {
        var sql = $@"DELETE FROM ""{nameof(CategoryModel)}"" WHERE ""{nameof(CategoryModel.Id)}"" = @Id";
        await _connection.ExecuteAsync(sql, new { Id = id });
    }
}