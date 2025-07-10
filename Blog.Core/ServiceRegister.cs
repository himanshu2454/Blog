using Blog.Core.Repository;
using Blog.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Core;

public static class CoreServiceRegister
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Post
        services.AddScoped(typeof(IPostRepo), typeof(PostRepo));
        services.AddScoped<IPostService, PostService>();

        //User
        services.AddScoped<IUserRepo, UserRepo>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
