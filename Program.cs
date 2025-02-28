using AssuredBid.Data;
using AssuredBid.Models;
using AssuredBid.Services;
using AssuredBid.Services.Iservice;
using AssuredBid.Services.UserServices;
using FluentEmail.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Net.Http.Headers;
using AssuredBid.Mappings;
using AssuredBid.Services.Implementation;

namespace AssuredBid
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Configure Swagger
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AssuredBid.API",
                    Version = "v1",
                    Description = "AssuredBid API that Generates tenders",
                });

                // Enable XML Documentation
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);

                // Enable JWT Authentication in Swagger
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and your valid token.\r\nExample: \"Bearer eyJhnbGciOrNwi78g...\""
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Configure PostgreSQL
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            // ✅ Add Identity Services (Fixes the 500 Error)
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Enable CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Configure JwtSettings
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

            // Register application services
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();

            // Configure JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });

            //Configure AutoMapper
            builder.Services.AddAutoMapper(typeof(Map));


            // Register Services
            builder.Services.AddTransient<ITenderService, TenderService>();
            builder.Services.AddTransient<ICompanyHouse, CompanyHouseService>();
            builder.Services.AddScoped<IUserService, UserService>();

            // Configure HTTP Client
            builder.Services.AddHttpClient("Assured_bid", options =>
            {
                options.Timeout = TimeSpan.FromSeconds(30);
                options.BaseAddress = new Uri("https://www.find-tender.service.gov.uk/api/1.0/");
            });

            builder.Services.AddHttpClient("Assured_bid2", op =>
            {
                op.Timeout = TimeSpan.FromSeconds(30);
                op.BaseAddress = new Uri("https://www.contractsfinder.service.gov.uk/");
            });

            builder.Services.AddHttpClient("Assured_bid3", op =>
            {
                op.Timeout = TimeSpan.FromSeconds(30);
                op.BaseAddress = new Uri("https://api.sell2wales.gov.wales/v1/");
            });

            builder.Services.AddHttpClient("Company_House", options =>
            {
                options.BaseAddress = new Uri("https://api.company-information.service.gov.uk/");
                options.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ebe61666-f8ff-4cfd-9210-86a5fdeac005");
            });

            builder.Services.AddHttpClient("Company_House", options =>
            {
                options.BaseAddress = new Uri("https://api-sandbox.company-information.service.gov.uk/");
                options.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("6df87760-2475-49aa-a2d5-cf7173e2f065");
            });

            var app = builder.Build();

            // Enable Swagger for both Local and Production
            app.UseSwagger();
            app.UseSwaggerUI();

            // Apply HTTPS redirection only in production
            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            // Apply CORS Policy
            app.UseCors("AllowAll");

            // Enable Middleware
            app.UseMiddleware<JwtBlacklistMiddleware>();

            // Authentication & Authorization Middleware
            app.UseAuthentication();
            app.UseAuthorization();

            // Map Controllers
            app.MapControllers();

            app.Run();
        }
    }
}
