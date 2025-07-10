using Blog.Persistance.Interfaces;
using Blog.Persistance.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Blog.Persistance.Repository;

public class MongoBaseRepo<T> : IMongoBaseRepo<T> where T : BaseEntity
{
    protected readonly IMongoCollection<T> Collection;

    protected MongoBaseRepo(IConfiguration config, string collection)
    {
        string connectionString = config.GetSection("MongoDbSettings:ConnectionString")?.Value ?? "";
        string databaseName = config.GetSection("MongoDbSettings:DatabaseName")?.Value ?? "";
        IMongoDatabase database = new MongoClient(connectionString).GetDatabase(databaseName);
        Collection = database.GetCollection<T>(collection);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await Collection.Find(x => true).ToListAsync();
    }

    public async Task<T> GetByIdAsync(Expression<Func<T, bool>> filter)
    {
        return await Collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T entity)
    {
        await Collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(Expression<Func<T, bool>> filter, T entity)
    {
        await Collection.ReplaceOneAsync(filter, entity);
    }

    public async Task DeleteAsync(Expression<Func<T, bool>> filter)
    {
        await Collection.DeleteOneAsync(filter);
    }
}

