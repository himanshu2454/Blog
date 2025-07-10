using Blog.Domain.Entities;
using Blog.Persistance.Interfaces;
using Blog.Persistance.Repository;
using Microsoft.Extensions.Configuration;

namespace Blog.Core.Repository;

public interface IPostRepo : IMongoBaseRepo<Post>
{
}

public class PostRepo : MongoBaseRepo<Post>, IPostRepo
{
    public PostRepo(IConfiguration config) : base(config, "Post")
    {
    }
}
