using Articles.Application.Articles;
using Articles.Application.Sections;
using Articles.Infrastructure.DataAccess.DbContext;
using Articles.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Articles.Infrastructure.DataAccess.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDataAccess(this IServiceCollection services)
    {
        services.AddDbContext<ArticleDbContext>(builder =>
        {
            builder
                .UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=ilmir;Password=qwerty;Pooling=false;Include Error Detail=true");
        });
        
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<ISectionRepository, SectionRepository>();
        
        return services;
    }
}