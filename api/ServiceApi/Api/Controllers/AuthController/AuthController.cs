using Application.Extensions;
using Application.UseCases.Auth.ChangePassword;
using Application.UseCases.Auth.ChangePassword.Request;
using Application.UseCases.Auth.CreateUser;
using Application.UseCases.Auth.CreateUser.Response;
using Application.UseCases.Auth.Login;
using Application.UseCases.Auth.Login.Request;
using Application.UseCases.Auth.Login.Response;
using Application.UseCases.Auth.RefreshToken;
using Application.UseCases.Auth.RefreshToken.Request;
using Application.UseCases.Auth.RefreshToken.Response;
using Application.UseCases.Auth.Register.Request;
using Application.UseCases.Auth.Register.Response;
using Application.UseCases.Auth.ResendPinCode;
using Application.UseCases.Auth.ResendPinCode.Request;
using Application.UseCases.Auth.RestoreForgotPassword;
using Application.UseCases.Auth.RestoreForgotPassword.Response;
using Application.UseCases.Auth.SignOut;
using Application.UseCases.Auth.SignOut.Response;
using Application.UseCases.Auth.VerifyEmail;
using Application.UseCases.Auth.VerifyEmail.Request;
using Application.UseCases.Auth.VerifyEmail.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AuthController;

/// <summary>
/// Контроллер для авторизации пользователя
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ILoginUseCase _loginUseCase;
    private readonly IRefreshTokenUseCase _refreshTokenUseCase;
    private readonly ISignOutUseCase _signOutUseCase;
    private readonly ICreateUserUseCase _createUserUseCase;
    private readonly IVerifyEmailUseCase _verifyEmailUseCase;
    private readonly IResendPinCodeUseCase _resendPinUseCase;
    private readonly IRestoreForgotPasswordUseCase _restoreForgotPasswordUseCase;
    private readonly IChangePasswordUseCase _changePasswordUseCase;
    
    public AuthController(
        ILoginUseCase loginUseCase,
        IRefreshTokenUseCase refreshTokenUseCase,
        ISignOutUseCase signOutUseCase,
        ICreateUserUseCase createUserUseCase, 
        IVerifyEmailUseCase verifyEmailUseCase, 
        IResendPinCodeUseCase resendPinCodeUseCase,
        IRestoreForgotPasswordUseCase restoreForgotPasswordUseCase,
        IChangePasswordUseCase changePasswordUseCase)
    {
        _loginUseCase = loginUseCase;
        _refreshTokenUseCase = refreshTokenUseCase;
        _signOutUseCase = signOutUseCase;
        _createUserUseCase = createUserUseCase;
        _verifyEmailUseCase = verifyEmailUseCase;
        _resendPinUseCase = resendPinCodeUseCase;
        _restoreForgotPasswordUseCase = restoreForgotPasswordUseCase;
        _changePasswordUseCase = changePasswordUseCase;
    }
    
    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginAsync(LoginRequest request)
    {
        var response = await _loginUseCase.ExecuteAsync(request);
        return response is LoginErrorResponse error ? BadRequest(error) : Ok(response);
    }

    /// <summary>
    /// Обновление Refresh токена
    /// </summary>
    [AllowAnonymous]
    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        return await _refreshTokenUseCase.ExecuteAsync(request);
    }

    /// <summary>
    /// Выход пользователя из ЛК, деактивация Refresh токена
    /// </summary>
    [Authorize]
    [HttpPost("sign-out")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<SignOutResponse> SignOutAsync()
    {
        var userId = User.GetUserId();
        return await _signOutUseCase.ExecuteAsync(userId);
    }

    /// <summary>
    /// Создание пользователя / регистрация
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<CreateUserResponse> RegisterAsync(CreateUserRequest request)
    {
        return await _createUserUseCase.ExecuteAsync(request);
    }
    
    /// <summary>
    /// Создание пользователя / Завершение регистрации
    /// </summary>
    [AllowAnonymous]
    [HttpPatch("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ContinueRegisterResponse> ContinueRegisterAsync(ContinueRegisterRequest request)
    {
        return await _createUserUseCase.ContinueRegisterAsync(request);
    }

    /// <summary>
    /// Верификация почты
    /// </summary>
    [AllowAnonymous]
    [HttpPost("verify-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<VerifyEmailResponse> SendEmailAsync(VerifyEmailRequest request)
    {
        var response = await _verifyEmailUseCase.ExecuteAsync(request);
        return response;
    }
    
    /// <summary>
    /// Переотправка пинкода верификации
    /// </summary>
    [AllowAnonymous]
    [HttpPost("resend-code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task ResendCodeAsync(ResendPinCodeRequest request)
    {
        await _resendPinUseCase.ExecuteAsync(request);
    }

    /// <summary>
    /// Прохождение аутентификации для изменения пароля
    /// </summary>
    [AllowAnonymous]
    [HttpPost("confirm-operation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<RestoreForgotPasswordResponse> RestoreForgotPasswordAsync(VerifyEmailRequest request)
    {
        return await _restoreForgotPasswordUseCase.ExecuteAsync(request);
    }
    
    /// <summary>
    /// Изменение пароля (забыли пароль)
    /// </summary>
    [Authorize]
    [HttpPost("change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task ChangePasswordAsync(ChangePasswordRequest request)
    {
        var userId = User.GetUserId();
        await _changePasswordUseCase.ExecuteAsync(userId, request);
    }
}