using System.Data;
using Application.Ports.Repositories;
using Dapper;
using Domain.Image;

namespace Infrastructure.Repositories.Images;

/// <inheritdoc />
internal class ImageRepository :  IImageRepository
{
    private readonly IDbConnection _connection;

    public ImageRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    /// <inheritdoc />
    public async Task<Guid> CreateAsync(ImageModel image)
    {
        var sql = $@"
            INSERT INTO ""{nameof(ImageModel)}""
            (
                ""{nameof(ImageModel.Id)}"",
                ""{nameof(ImageModel.ProductId)}"",
                ""{nameof(ImageModel.ObjectPath)}"",
                ""{nameof(ImageModel.IsMain)}""
            )
            VALUES
            (
                @Id,
                @ProductId,
                @ObjectPath,
                @IsMain
            );";

        await _connection.ExecuteAsync(sql, new
        {
            image.Id,
            image.ProductId,
            image.ObjectPath,
            image.IsMain
        });

        return image.Id;
    }

    /// <inheritdoc />
    public async Task<ImageModel> GetByIdAsync(Guid id)
    {
        var sql = $@"
            SELECT *
            FROM ""{nameof(ImageModel)}""
            WHERE ""{nameof(ImageModel.Id)}"" = @Id;";

        var image = await _connection.QueryFirstOrDefaultAsync<ImageModel>(sql, new { Id = id });

        if (image == null)
            throw new InvalidOperationException("Изображение не найдено");

        return image;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ImageModel>> GetAllByProductIdAsync(Guid productId)
    {
        var sql = $@"
            SELECT *
            FROM ""{nameof(ImageModel)}""
            WHERE ""{nameof(ImageModel.ProductId)}"" = @ProductId
            ORDER BY ""{nameof(ImageModel.IsMain)}"" DESC;";

        return await _connection.QueryAsync<ImageModel>(sql, new { ProductId = productId });
    }

    /// <inheritdoc />
    public async Task<ImageModel> GetMainImageAsync(Guid productId)
    {
        var sql = $@"
            SELECT *
            FROM ""{nameof(ImageModel)}""
            WHERE ""{nameof(ImageModel.ProductId)}"" = @ProductId
              AND ""{nameof(ImageModel.IsMain)}"" = TRUE
            LIMIT 1;";

        var image = await _connection.QueryFirstOrDefaultAsync<ImageModel>(sql, new { ProductId = productId });

        if (image == null)
            throw new InvalidOperationException("Основное изображение не найдено");

        return image;
    }

    /// <inheritdoc />
    public async Task<ImageModel> UpdateAsync(ImageModel image)
    {
        var sql = $@"
            UPDATE ""{nameof(ImageModel)}""
            SET ""{nameof(ImageModel.ObjectPath)}"" = @ObjectPath,
                ""{nameof(ImageModel.IsMain)}"" = @IsMain
            WHERE ""{nameof(ImageModel.Id)}"" = @Id;";

        await _connection.ExecuteAsync(sql, new
        {
            image.Id,
            image.ObjectPath,
            image.IsMain
        });

        return image;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        var sql = $@"
            DELETE FROM ""{nameof(ImageModel)}""
            WHERE ""{nameof(ImageModel.Id)}"" = @Id;";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }
}