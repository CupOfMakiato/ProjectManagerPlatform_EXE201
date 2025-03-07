using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Application.Interfaces;
using Server.Application.Repositories;
using Server.Application.Services;
using Server.Application;
using Server.Contracts.Settings;
using Server.Infrastructure.Data;
using Server.Infrastructure.Repositories;
using Server.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Server.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //UOW
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Service
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISubCategoryService, SubCategoryService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBoardService, BoardService>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<IColumnsService, ColumnService>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<PasswordService>();
            services.AddScoped<OtpService>();
            services.AddScoped<EmailService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();

            services.AddMemoryCache();

            // Repo
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBoardRepository, BoardRepository>();
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<IColumnRepository, ColumsRepository>();
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            //
            services.Configure<CloudinarySetting>(configuration.GetSection("CloudinarySetting"));

            #region Configuration

            #endregion
            // Database Sql
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")
                ));
            return services;
        }
    }
}
