using Jumia_Api.Application.Interfaces;
using Jumia_Api.Application.MappingProfiles;
using Jumia_Api.Application.Services;
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
            // Scan all assemblies
            services.AddAutoMapper(cfg => { },typeof(ProductsMapping).Assembly);
            services.AddScoped<IAuthService, AuthService>();




            return services;

        }
    }
}
