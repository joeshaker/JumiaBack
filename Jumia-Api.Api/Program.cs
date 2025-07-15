
using Jumia_Api.Api.DependencyInjection.Application;
using Jumia_Api.Api.DependencyInjection.Domain;
using Jumia_Api.Api.DependencyInjection.Infrastructure;
using Jumia_Api.Application.Interfaces;
using Jumia_Api.Application.Services;
using Jumia_Api.Domain.Interfaces.UnitOfWork;
using Jumia_Api.Infrastructure.Presistence.UnitOfWork;
using Microsoft.OpenApi.Models;

namespace Jumia_Api.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddDomain(builder.Configuration);
            builder.Services.AddApplication(builder.Configuration);
           
            // Swagger/OpenAPI configuration
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Jumia API",
                    Version = "v1",
                    Description = "API for Jumia Application",
                });
            });
           
          

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jumia API v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
