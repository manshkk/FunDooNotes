using FunDooNotes.RabbitMQ;
using StackExchange.Redis;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Repositories;
using RepositoryLayer.Services;
using System.Text;


namespace FunDooNotes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Controllers
            builder.Services.AddControllers();

            // JWT Authentication
            builder.Services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer =
                                builder.Configuration["Jwt:Issuer"],

                            ValidAudience =
                                builder.Configuration["Jwt:Audience"],

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(
                                        builder.Configuration["Jwt:Key"]))
                        };

                    // JWT Error Logging
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine(
                                "JWT Error: " +
                                context.Exception.Message);

                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddEndpointsApiExplorer();

            // Swagger JWT Configuration
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description =
                            "Enter JWT Token Only"
                    });

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference =
                                    new OpenApiReference
                                    {
                                        Type =
                                            ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                            },
                            Array.Empty<string>()
                        }
                    });
            });

            // Database Context
            builder.Services.AddDbContext<FundooContext>(
                options =>
                {
                    options.UseSqlServer(
                        builder.Configuration.GetConnectionString(
                            "DefaultConnection"));
                });
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration =
                    builder.Configuration["RedisURL"];

                return ConnectionMultiplexer.Connect(configuration);
            });

            // Dependency Injection

            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<ICacheService, CacheService>();

            builder.Services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();

            builder.Services.AddHostedService<EmailConsumerService>();

            builder.Services.AddHostedService<NoteConsumerService>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<ITokenService, TokenService>();

            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddScoped<INoteRepository, NoteRepository>();

            builder.Services.AddScoped<INoteService, NoteService>();

            builder.Services.AddScoped<ILabelRepository, LabelRepository>();

            builder.Services.AddScoped< ILabelService,LabelService>();

            builder.Services.AddScoped<ICollaboratorRepository, CollaboratorRepository>();

            builder.Services.AddScoped< ICollaboratorService, CollaboratorService>();

            var app = builder.Build();

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            // Authentication First
            app.UseAuthentication();

            // Authorization Second
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}