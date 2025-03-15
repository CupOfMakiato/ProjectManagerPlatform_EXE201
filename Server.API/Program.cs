using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server.API;
using Server.API.Middlewares;
using Server.Application;
using Server.Application.Interfaces;
using Server.Infrastructure;
using Server.Infrastructure.Hubs;
using Server.Infrastructure.Services;
using System.Diagnostics;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.


{
    builder.Services
        .AddApi()
        .AddApplication()
        .AddInfrastructure(builder.Configuration);
}

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add Rate Limiting
//builder.Services.AddRateLimiter(options =>
//{
//    options.AddFixedWindowLimiter("fixed", limiterOptions =>
//    {
//        limiterOptions.Window = TimeSpan.FromSeconds(10);  // 10-sec time window
//        limiterOptions.PermitLimit = 5;  // Allow max 5 requests per window
//        limiterOptions.QueueLimit = 2;   // Queue up to 2 extra requests
//    });
//});

// Configure JWT authentication
var key = System.Text.Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:SecretKey"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            ClockSkew = System.TimeSpan.Zero
        };
    })
    .AddCookie()
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = builder.Configuration["GoogleAPI:ClientId"];
        options.ClientSecret = builder.Configuration["GoogleAPI:SecretCode"];
    });

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Kestrel File Upload Size

//var maxFileSize = builder.Configuration.GetValue<long>("Kestrel:Limits:MaxRequestBodySize");
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.Limits.MaxRequestBodySize = maxFileSize;
//});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();


var app = builder.Build();


app.UseSwagger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwaggerUI();
}
else
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.InjectJavascript("/custom-swagger.js");
        c.RoutePrefix = string.Empty;
    });
}

app.UseExceptionHandler("/Error");

app.UseCors("AllowAllOrigins");




// Middleware for performance tracking
app.UseMiddleware<PerformanceMiddleware>();


app.UseHttpsRedirection();

// use authen
app.UseAuthentication();
app.UseAuthorization();

// Use Global Exception Middleware 
app.UseMiddleware<GlobalExceptionMiddleware>();
app.MapControllers();

app.MapHub<NotificationHub>("hub/notificationHub");
app.UseSqlTableDependency(builder.Configuration.GetConnectionString("DefaultConnection"));


app.Run();
