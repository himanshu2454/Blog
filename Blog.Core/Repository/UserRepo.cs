using Blog.Domain.Entities;
using Blog.Persistance.Interfaces;
using Blog.Persistance.Repository;
using Microsoft.Extensions.Configuration;

namespace Blog.Core.Repository;

public interface IUserRepo : IMongoBaseRepo<User>
{
}

public class UserRepo : MongoBaseRepo<User>, IUserRepo
{
    public UserRepo(IConfiguration config) : base(config, "User")
    {
    }
}
