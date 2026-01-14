using Application.Exceptions.Image;
using Application.Exceptions.Product;
using Application.Ports.Repositories;
using Application.UseCases.Image.Dto;
using Application.UseCases.Image.Interfaces;
using Domain.Image;
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
    
    private static readonly string[] AllowedContentTypes =
    {
        "image/jpeg",
        "image/png",
        "image/webp"
    };


    public ImageUseCase(IProductRepository productRepository, IImageRepository imageRepository, IMinioClient minio, IConfiguration configuration)
    {
        _productRepository = productRepository;
        _imageRepository = imageRepository;
        _minio = minio;
        _bucket = configuration["Minio:Bucket"]!;
        _publicUrl = configuration["Minio:PublicUrl"]!;
    }

    /// <inheritdoc />
    public async Task AddProductImageAsync(Guid productId, IFormFile file, bool isMain)
    {
        ValidateFile(file); 
        
        var product = await _productRepository.GetProductAsync(productId);
        if (product == null)
        {
            throw new ProductNotFoundException("Product not found");
        }

        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var objectPath = $"products/{productId}/{fileName}";
        try
        {
            // Загрузка в MinIO
            await _minio.PutObjectAsync(
                new PutObjectArgs()
                    .WithBucket(_bucket)
                    .WithObject(objectPath)
                    .WithStreamData(file.OpenReadStream())
                    .WithObjectSize(file.Length)
                    .WithContentType(file.ContentType));
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
            };

            await _imageRepository.CreateAsync(image);
        }
        catch
        {
            await _minio.RemoveObjectAsync(
                new RemoveObjectArgs()
                    .WithBucket(_bucket)
                    .WithObject(objectPath));
            throw new ImageCreateException("Произошла ошибка при загрузке изображения");
        }
        
    }

    /// <inheritdoc />
    public async Task DeleteProductImageAsync(Guid productId, Guid imageId)
    {
        var image = await _imageRepository.GetByIdAsync(imageId);
        if (image == null || image.ProductId != productId)
        {
            throw new ImageNotFoundException("Файл не найден");
        }

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
        return images.Select(i => new ProductImageResponse {
        Id = i.Id,
        Url = $"{_publicUrl}/{i.ObjectPath}",
        IsMain = i.IsMain
    });
}

    /// <summary>
    /// Валидация полученного на загрузку файла
    /// </summary>
    private static void ValidateFile(IFormFile file)
    {
        if (file == null)
            throw new InvalidImageException("Файл не передан");

        if (file.Length == 0)
            throw new InvalidImageException("Файл пустой");

        if (file.Length > 5_000_000)
            throw new InvalidImageException("Размер файла превышает 5MB");

        if (!AllowedContentTypes.Contains(file.ContentType))
            throw new InvalidImageException("Недопустимый тип файла");
    }
}





