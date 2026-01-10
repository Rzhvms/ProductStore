using System.Data;
using Application.Ports.Repositories;
using Domain.Image;

namespace Infrastructure.Repositories.Images;

internal class ImageRepository :  IImageRepository
{
    private readonly IDbConnection _connection;

    public ImageRepository(IDbConnection connection)
    {
        _connection = connection;
    }
    public Task<Guid> CreateAsync(ImageModel image)
    {
        throw new NotImplementedException();
    }

    public Task<ImageModel> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ImageModel>> GetAllByProductIdAsync(Guid productId)
    {
        throw new NotImplementedException();
    }

    public Task<ImageModel> GetMainImageAsync(Guid productId)
    {
        throw new NotImplementedException();
    }

    public Task<ImageModel> UpdateAsync(ImageModel image)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}