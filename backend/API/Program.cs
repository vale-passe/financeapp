using System.Text;
using API.Features.Auth.Models;
using API.Features.Auth.Services;
using API.Features.Auth.Services.Interfaces;
using API.Features.Auth.Validators;
using API.Infrastructure.Data;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

namespace API;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddCors();
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddScoped<ITokenService, TokenService>();

        builder.Services.AddScoped<IValidator<RegisterModel>, RegisterModelValidator>();
        builder.Services.AddScoped<IValidator<LoginModel>, LoginModelValidator>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var tokenKey = builder.Configuration["TokenKey"]
                               ?? throw new Exception("TokenKey not found - Program.cs");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseCors(options => options
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("http://localhost:4200", "https://localhost:4200"));
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        
        app.Run();
    }
}