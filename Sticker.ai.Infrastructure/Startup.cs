using Microsoft.AspNetCore.Builder;
using StickerAI.Infrastructure.Models;
using StickerAI.Infrastructure.Configuration;
using StickerAI.Infrastructure.Interfaces;
using StickerAI.Infrastructure.Services;
using StickerAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using StickerAI.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using StickerAI.Infrastructure.Extensions;

namespace StickerAI.Infrastructure;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Configure settings
        services.Configure<AppSettings>(_configuration.GetSection("AppSettings"));

        // Add database context
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

        // Register services
        services.AddScoped<IStickerRepository, StickerRepository>();
        services.AddScoped<IStickerGenerator, StickerGenerator>();
        services.AddScoped<StickerService>();

        // Add controllers
        services.AddControllers();

        // Add Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sticker.ai API", Version = "v1" });
        });

        // Add CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionMiddleware();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sticker.ai API v1"));
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}