using Articles.Application.Articles;
using Articles.Application.Sections;
using Articles.Infrastructure.DataAccess.DbContext;
using Articles.Infrastructure.DataAccess.Repositories;
using Articles.Infrastructure.DataAccess.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Articles.Infrastructure.DataAccess.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ArticleDbContext>(builder =>
        {
            builder
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        
        services.AddScoped<IArticleUnitOfWork, ArticleUnitOfWork>();
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<ISectionRepository, SectionRepository>();
        
        return services;
    }
}