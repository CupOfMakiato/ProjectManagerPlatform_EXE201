using Microsoft.Extensions.DependencyInjection;
using Server.API.Middlewares;
using Server.API.Services;
using Server.Application.Interfaces;
using Server.Domain.Enums;
using System.Collections.Generic;
using System.Diagnostics;

namespace Server.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler =
                        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition =
                        System.Text.Json.Serialization.JsonIgnoreCondition.Never;
                    options.JsonSerializerOptions.Converters.Add(
                        new System.Text.Json.Serialization.JsonStringEnumConverter()); // Add this line to handle enums as strings in JSON
                });
            services.AddSignalR();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<GlobalExceptionMiddleware>();
            services.AddScoped<Stopwatch>();
            services.AddScoped<PerformanceMiddleware>();
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Project Manager Platform Web App API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' followed by space and JWT",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
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
                    new string[] { }
                }
            });


                c.MapType<CardProviderEnum>(() => new Microsoft.OpenApi.Models.OpenApiSchema
                {
                    Type = "string",
                    Enum = new List<Microsoft.OpenApi.Any.IOpenApiAny>
                {
                    new Microsoft.OpenApi.Any.OpenApiString(CardProviderEnum.Visa.ToString()),
                    new Microsoft.OpenApi.Any.OpenApiString(CardProviderEnum.MasterCard.ToString()),
                    new Microsoft.OpenApi.Any.OpenApiString(CardProviderEnum.AmericanExpress.ToString()),
                    new Microsoft.OpenApi.Any.OpenApiString(CardProviderEnum.Discover.ToString())
                }
                });
            });



            return services;
        }
    }
}
