using Application.Enums;
using Application.Ports.Repositories;
using Application.Ports.Services;
using Application.Ports.Services.Email;
using Application.UseCases.Auth.CreateUser;
using Application.UseCases.Auth.CreateUser.Request;
using Application.UseCases.Auth.CreateUser.Response;
using Application.UseCases.Auth.Register.Request;
using Application.UseCases.Auth.Register.Response;
using Domain.User;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Auth.Register;

/// <inheritdoc />
internal class CreateUserUseCase : ICreateUserUseCase
{
    private readonly ILogger<CreateUserUseCase> _logger;
    private readonly IAuthRepository _authRepository;
    private readonly ICryptographyService _cryptographyService;
    private readonly IEmailService _emailService;

    public CreateUserUseCase(
        ILogger<CreateUserUseCase> logger,
        IAuthRepository authRepository,
        ICryptographyService cryptographyService,
        IEmailService emailService)
    {
        _logger = logger;
        _authRepository = authRepository;
        _cryptographyService = cryptographyService;
        _emailService = emailService;
    }

    /// <inheritdoc />
    public async Task<CreateUserResponse> ExecuteAsync(CreateUserRequest request)
    {
        // Проверяем, не существует ли пользователь с таким email
        var existingUser = await _authRepository.GetUserByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new CreateUserErrorResponse
            {
                Message = "Пользователь с таким email уже существует.",
                Code = ErrorCodes.UserDoesNotExist.ToString("D")
            };
        }

        var salt = _cryptographyService.GenerateSalt();
        var now = DateTime.UtcNow;

        var user = new UserModel
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Password = _cryptographyService.HashPassword(request.Password, salt),
            Salt = salt,
            Claims = ToClaims(request.Claims),
            CreateAt = now,
            UpdateAt = now
        };

        await _authRepository.CreateUserAsync(user);
        
        await _emailService.SendVerificationEmail(user.Email);
        _logger.LogInformation("Пользователь {Email} успешно создан", user.Email);

        return new CreateUserSuccessResponse
        {
            UserId = user.Id
        };
    }
    
    /// <inheritdoc />
    public async Task<ContinueRegisterResponse> ContinueRegisterAsync(ContinueRegisterRequest request)
    {
        await _authRepository.UpdateUserForFinalRegistrationAsync(request.UserId, request.Name, request.LastName,
            request.Gender, request.Phone, request.BirthDate);
        return new ContinueRegisterResponse();
    }

    /// <summary>
    /// Преобразует список claim из запроса в доменные объекты.
    /// </summary>
    private static IList<ClaimModel> ToClaims(IList<Claim>? requestClaims)
    {
        if (requestClaims == null || requestClaims.Count == 0)
            return new List<ClaimModel>();

        return requestClaims
            .Select(c => new ClaimModel
            {
                Type = c.Type,
                Value = c.Value
            })
            .ToList();
    }
}