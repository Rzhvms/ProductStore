using Application.UseCases.AddressSuggest;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AddressController;

[ApiController]
[Route("api/address")]
public sealed class AddressController : ControllerBase
{
    private readonly IAddressSuggestUseCase _useCase;

    public AddressController(IAddressSuggestUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpGet("suggest")]
    public async Task<IActionResult> Suggest([FromQuery] string q)
    {
        var result = await _useCase.ExecuteAsync(q);
        return Ok(result);
    }
}
