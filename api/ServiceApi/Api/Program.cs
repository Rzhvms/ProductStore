using System.Security.Cryptography;
using Api.Middlewares;
using Application;
using Application.Configuration;
using FluentMigrator.Runner;
using Infrastructure;
using Infrastructure.Services.Auth.Jwt;
using Infrastructure.Services.Email.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

# region Infrastructure

var redisConnStr = builder.Configuration.GetValue<string>("Redis:ConnectionString") ?? "localhost:6379";

// Регистрируем IConnectionMultiplexer как Singleton
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(redisConnStr)
);

// Конфигурация JWT
var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);
var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

// Читаем строку подключения из appsettings
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Настройка CORS
builder.Services.AddCors(options =>
{
    // options.AddPolicy("AllowSpecificOrigin", policy =>
    // {
    //     policy.WithOrigins(
    //         // TODO: указать допустимые адреса
    //         .AllowAnyHeader()
    //         .AllowAnyMethod()
    //         .AllowCredentials();
    // });

    // Разрешаем все для разработки
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Подключаем настройки Email
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddInfrastructureModule(jwtSettings, connectionString);
# endregion 

# region Application
builder.Services.AddApplicationModule();
# endregion

// Добавление контроллеров и Swagger
builder.Services.AddControllers();

builder.Services.AddMemoryCache();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ProductStore.Api",
        Version = "v1"
    });

    // Swagger + JWT
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});

// JWT Authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem(jwtSettings.AccessTokenSettings.PublicKey);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.AccessTokenSettings.Issuer,
            ValidAudience = jwtSettings.AccessTokenSettings.Audience,
            IssuerSigningKey = new RsaSecurityKey(rsa),
            ClockSkew = TimeSpan.Zero
        };
    });

// Конфигурация из appsettings.json
builder.Services.Configure<AppOptions>(builder.Configuration.GetSection("App"));
builder.Services.Configure<YandexAddressSuggestServiceOptions>(builder.Configuration.GetSection("Yandex"));


var app = builder.Build();

// Middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();

app.UseRouting();

// Используем CORS политику
// app.UseCors("AllowSpecificOrigin");
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Роутинг
app.MapControllers();

// Авто-запуск миграций FluentMigrator
using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

// Запуск
app.Run();