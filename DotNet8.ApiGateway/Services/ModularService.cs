using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using DotNet8.POS.Shared;

namespace DotNet8.POS.ApiGateway.Services;

public static class ModularService
{
    public static IServiceCollection AddModularServices(this WebApplicationBuilder builder)
    {
        return builder
            .AddConfiguration()
            .AddJwtAuthentication()
            .AddSwaggerGen()
            .AddHttpClientService()
            .AddScopedService()
            .Services;
    }

    private static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
    {
        var builderAppSetting = new ConfigurationBuilder();
        builderAppSetting.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        IConfiguration configAppSetting = builderAppSetting.Build();
        builder.Services.AddOptions();

        return builder;
    }

    private static WebApplicationBuilder AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        #region JwtBearer Authentication

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                };
            });

        #endregion

        return builder;
    }

    private static WebApplicationBuilder AddSwaggerGen(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        #region Authorization Swagger UI

        builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "PosSystem", Version = "1.0.0" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string> ()
                }
            });
        });

        #endregion

        return builder;
    }

    private static WebApplicationBuilder AddHttpClientService(this WebApplicationBuilder builder)
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
        };

        builder.Services.AddHttpClient("PosService", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["PosServiceUrl"]);
            client.Timeout = TimeSpan.FromMinutes(5);
        }).ConfigurePrimaryHttpMessageHandler(() => handler);

        builder.Services.AddHttpClient("PointService", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["PointServiceUrl"]);
            client.Timeout = TimeSpan.FromMinutes(5);
        }).ConfigurePrimaryHttpMessageHandler(() => handler);

        builder.Services.AddHttpClient("CmsService", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["CmsServiceUrl"]);
            client.Timeout = TimeSpan.FromMinutes(5);
        }).ConfigurePrimaryHttpMessageHandler(() => handler);

        builder.Services.AddScoped<HttpClientService>();

        return builder;
    }

    private static WebApplicationBuilder AddScopedService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<JwtTokenService>();
        return builder;
    }
}