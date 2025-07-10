using Blog.Persistance.Interfaces;
using Blog.Persistance.Models;
using Blog.Persistance.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Blog.Persistance;

[ExcludeFromCodeCoverage]
public static class PersistanceServiceRegister
{
    public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
        services.AddScoped(typeof(IMongoBaseRepo<>), typeof(MongoBaseRepo<>));
        return services;
    }   
}