using Application.TypeHandlers;
using Application.UseCases.Auth.ChangePassword;
using Application.UseCases.Auth.CreateUser;
using Application.UseCases.Auth.Login;
using Application.UseCases.Auth.RefreshToken;
using Application.UseCases.Auth.Register;
using Application.UseCases.Auth.ResendPinCode;
using Application.UseCases.Auth.RestoreForgotPassword;
using Application.UseCases.Auth.SignOut;
using Application.UseCases.Auth.VerifyEmail;
using Application.UseCases.Cart;
using Application.UseCases.Category;
using Application.UseCases.Image;
using Application.UseCases.Image.Interfaces;
using Application.UseCases.Order;
using Application.UseCases.Payment;
using Application.UseCases.Product;
using Application.UseCases.Product.Interfaces;
using Application.UseCases.User.ChangeUserPassword;
using Application.UseCases.User.DeleteUser;
using Application.UseCases.User.GetUserInfo;
using Application.UseCases.User.UpdateUserInfo;
using Application.UseCases.UserAddress;
using Application.UseCases.UserProfile;
using Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationForStartup
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services)
    {
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();
        services.AddScoped<ISignOutUseCase, SignOutUseCase>();
        services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
        services.AddScoped<IChangeUserPasswordUseCase, ChangeUserPasswordUseCase>();
        services.AddScoped<IGetUserInfoUseCase, GetUserInfoUseCase>();
        services.AddScoped<IUpdateUserInfoUseCase, UpdateUserInfoUseCase>();
        services.AddScoped<IUserAddressUseCase, UserAddressUseCase>();
        services.AddScoped<IVerifyEmailUseCase, VerifyEmailUseCase>();
        services.AddScoped<IResendPinCodeUseCase, ResendPinCodeUseCase>();
        services.AddScoped<IUserProfileUseCase, UserProfileUseCase>();
        services.AddScoped<ICategoryUseCase, CategoryUseCase>();
        services.AddScoped<IProductUseCase, ProductUseCase>();
        services.AddScoped<IImageUseCase, ImageUseCase>();
        services.AddScoped<ICartUseCase, CartUseCase>();
        services.AddScoped<IRestoreForgotPasswordUseCase, RestoreForgotPasswordUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
        services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
        services.AddScoped<IOrderUseCase, OrderUseCase>();
        services.AddScoped<IPaymentUseCase, PaymentUseCase>();
        services.AddScoped<IFavoriteProductsUseCase, FavoriteProductsUseCase>();
        services.AddScoped<IProductReviewUseCase, ProductReviewUseCase>();
        
        SqlMapper.AddTypeHandler(new JsonObjectTypeHandler());
        SqlMapper.AddTypeHandler(new JsonArrayTypeHandler());

        return services;
    }
}