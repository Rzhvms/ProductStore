namespace Application.UseCases.Image.Dto;

public record ProductImageResponse
{
    public Guid Id { get; init; }

    public string? Url { get; init; }

    public bool IsMain { get; init; }

}