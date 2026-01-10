using Application.Ports.Repositories;
using Application.UseCases.Image.Interfaces;
using Application.UseCases.Product.Interfaces;
using Domain.Image;
using Domain.Product.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace Application.UseCases.Image;

/// <inheritdoc />
public class ImageUseCase : IImageUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IMinioClient _minio;
    private readonly string _bucket;
    private readonly string _publicUrl;

    public ImageUseCase(IProductRepository productRepository, IImageRepository imageRepository, IMinioClient minio, IConfiguration configuration)
    {
        _productRepository = productRepository;
        _imageRepository = imageRepository;
        _minio = minio;
        _bucket = configuration["Minio:Bucket"]!;
        _publicUrl = configuration["Minio:PublicUrl"]!;
    }

    /// <inheritdoc />
    public async Task AddProductImageAsync(Guid productId, IFormFile file, bool isMain, ImageSortOrder sortOrder)
    {
        var product = await _productRepository.GetProductAsync(productId);
        if (product == null)
        {
            throw new ("Product not found");
        }

        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var objectPath = $"products/{productId}/{fileName}";

        // Загрузка в MinIO
        await _minio.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(_bucket)
                .WithObject(objectPath)
                .WithStreamData(file.OpenReadStream())
                .WithObjectSize(file.Length)
                .WithContentType(file.ContentType)
        );

        if (isMain)
        {
            var existingMain = await _imageRepository.GetMainImageAsync(productId);
            existingMain.IsMain = false;
            await _imageRepository.UpdateAsync(existingMain);
        }

        var image = new ImageModel
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            ObjectPath = objectPath,
            IsMain = isMain,
            SortOrder = sortOrder
        };

        await _imageRepository.CreateAsync(image);
    }

    /// <inheritdoc />
    public async Task DeleteProductImageAsync(Guid productId, Guid imageId)
    {
        var image = await _imageRepository.GetByIdAsync(imageId);
        if (image == null || image.ProductId != productId)
            throw new KeyNotFoundException("Image not found");

        // Удаление из MinIO
        await _minio.RemoveObjectAsync(
            new RemoveObjectArgs()
                .WithBucket(_bucket)
                .WithObject(image.ObjectPath)
        );

        await _imageRepository.DeleteAsync(imageId);
    }
    
    public async Task<IEnumerable<ProductImageResponse>> GetProductImagesAsync(Guid productId)
    {
        var images = await _imageRepository.GetAllByProductIdAsync(productId);
        return images.Select(i => ProductImageResponse.Create(
            i.Id,
            $"{_publicUrl}/{i.ObjectPath}",
            i.IsMain,
            i.SortOrder));
    }
}

public record ProductImageResponse
{
    private Guid Id { get; init; }

    private string? Url { get; init; }

    private bool IsMain { get; init; }

    private ImageSortOrder SortOrder { get; init; }

    public static ProductImageResponse Create(Guid id, string url, bool isMain, ImageSortOrder? sortOrder)
    {
        return new ProductImageResponse
        {
            Id = id,
            Url = url,
            IsMain = isMain,
            SortOrder = sortOrder ?? 0
        };
    }
}

