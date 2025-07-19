using Jumia_Api.Application.Interfaces;
using Jumia_Api.Application.MappingProfiles;
using Jumia_Api.Application.Services;
using Jumia_Api.Domain.Interfaces.Repositories;
using Jumia_Api.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Jumia_Api.Api.DependencyInjection.Application
{
    public static class ApplicationServicesRegistraion
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOrderService, OrderService>();
            // Scan all assemblies
            services.AddAutoMapper(cfg => { },typeof(ProductsMapping).Assembly);
            services.AddAutoMapper(cfg => { }, typeof(UserMapping).Assembly);
            services.AddAutoMapper(cfg => { }, typeof(OrderMapping).Assembly);
            services.AddAutoMapper(cfg => { }, typeof(WishlistMapping).Assembly);
            services.AddScoped<IAuthService, AuthService>();


            services.AddMemoryCache();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IWishlistService, WishlistService>();





            return services;

        }
    }
}
