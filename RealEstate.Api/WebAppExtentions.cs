using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RealEstate.Api.Middleware;
using RealEstate.BLL;
using RealEstate.DAL;
using RealEstate.DAL.Repositories;
using RealEstate.Domain;
using RealEstate.Domain.Contracts.Data;
using RealEstate.Domain.Contracts.Services;
using RealEstate.Domain.Entities;
using System.Text;

namespace RealEstate.Api
{
    public static class WebAppExtentions
    {
        public static WebApplicationBuilder BuildServices(this WebApplicationBuilder builder)
        {
            // Configurations
            // Home Assignment: 
            // o Use environment variables for config(e.g., DB connection) 
            builder.Services.Configure<AppSettings>(builder.Configuration);
            var appSettings = builder.Configuration.Get<AppSettings>() ?? throw new Exception("Configuration Not Found");

            //DB
            builder.Services.AddDbContext<RealEstateContext>(options =>
            {
                options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection);
            });

            // Home Assignment: 
            // o	Optional (for extra credit): Basic auth, rate limiting, or Dockerization.
            // Auth with default ms identity
            BuildAuth(builder, appSettings);
            
            // Corses
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost",
                    builder =>
                    {
                        builder.WithOrigins(appSettings.AllowedHosts)
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            // Home Assignment: 
            // o	API docs (e.g., curl examples or Swagger).
            BuildSwagger(builder);

            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();
            // Corses
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost",
                    builder =>
                    {
                        builder.WithOrigins(appSettings.AllowedHosts)
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            // AutoMapper 
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.Services.AddAutoMapper(cfg => cfg.AddMaps(assemblies));

            //DAL
            builder.Services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
            builder.Services.AddTransient<IPropertyRepository, PropertyRepository>();
            builder.Services.AddTransient<ISpaceRepository, SpaceRepository>();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

            //BLL
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IPropertyService, PropertyService>();
            builder.Services.AddScoped<ISpaceService, SpaceService>();
            builder.Services.AddScoped<IStatsService, StatsService>();

            return builder;
        }

        private static void BuildSwagger(WebApplicationBuilder builder)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    // JWT Authentication
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "JWT token: Bearer {your_token}"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                            },
                            Array.Empty<string>()
                        }
                    });
                });
            }
        }

        public static WebApplication BuildPipleline(this WebApplication app)
        {
            //Home Assignment: 
            // use custom exception handler middleware
            app.UseExceptionHandlerMiddleware();
            //Home Assignment: 
            // default microsooft rate limiter bellow
            // app.UseRateLimiter();
            // or we can use our own custom rate limiter bellow
            app.UseCustomRateLimiter();

            app.UseCors("AllowLocalhost");

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");
            return app;
        }

        ////Home Assignment: 
        public static void BuildAuth(WebApplicationBuilder builder, AppSettings appSettings)
        {
            builder.Services.AddIdentity<User, IdentityRole>()
               .AddEntityFrameworkStores<RealEstateContext>()
               .AddDefaultTokenProviders();
            var key = Encoding.UTF8.GetBytes(appSettings.Jwt.Key);

            builder.Services.AddAuthorization();//According requriement we donnt need to configure policies

            builder.Services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
        }

        public static WebApplication SeedDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var databaseInitializer = services.GetRequiredService<IDatabaseInitializer>();
                databaseInitializer.SeedAsync().Wait();
            }
            return app;
        }

      

    }
}
