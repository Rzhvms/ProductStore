using Application.UseCases.Product;
using Domain.Image;
using Domain.Product;

namespace Application.Ports.Repositories;

public interface IImageRepository
{
    Task<Guid> CreateAsync(ImageModel image);
    Task<ImageModel> GetByIdAsync(Guid id);
    Task<IEnumerable<ImageModel>> GetAllByProductIdAsync(Guid productId);
    Task<ImageModel> GetMainImageAsync(Guid productId);
    Task<ImageModel> UpdateAsync(ImageModel image);
    Task DeleteAsync(Guid id);
}