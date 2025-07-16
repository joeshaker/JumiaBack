
using Jumia_Api.Api.DependencyInjection.Application;
using Jumia_Api.Api.DependencyInjection.Domain;
using Jumia_Api.Api.DependencyInjection.Infrastructure;
using Jumia_Api.Application.Interfaces;
using Jumia_Api.Application.Services;

using Jumia_Api.Domain.Models;
using Jumia_Api.Infrastructure.Presistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            builder.Services.AddControllers();

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddDomain(builder.Configuration);
            builder.Services.AddApplication(builder.Configuration);



            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<JumiaDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<IAuthService, AuthService>();

            var jwtConfig = builder.Configuration.GetSection("Jwt");
            //builder.Services.Configure<JwtOptions>(jwtConfig);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtConfig["Issuer"],
                    ValidAudience = jwtConfig["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtConfig["Key"]!)),
                    //ClockSkew = TimeSpan.Zero

                };
                //options.Events = new JwtBearerEvents
                //{
                //    OnMessageReceived = context =>
                //    {
                //        var token = context.Request.Cookies["jwt"];
                //        if (!string.IsNullOrEmpty(token))
                //        {
                //            context.Token = token;
                //        }
                //        return Task.CompletedTask;
                //    }
                //};

            });


            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();

       
           
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

            //Enable Swagger middleware
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jumia API v1");
                });

            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
