using Blog.Core.Repository;
using Blog.Domain.Entities;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Blog.Core.Services;

public interface IPostService
{
    Task<Result<bool>> CreateAsync(Post post);
    Task<Result<Post>> GetByIdAsync(string id);
    Task<Result<List<Post>>> GetAllAsync();
    Task<Result<bool>> UpdateAsync(Post post);
    Task<Result<bool>> DeleteAsync(Guid id);
    Task<Result<Post>> GetByIdAsyncAggressiveInlining(string id);
}

public class PostService : IPostService
{
    private readonly IPostRepo _postRepository;
    private readonly ILogger<PostService> _logger;

    public PostService(IPostRepo postRepository, ILogger<PostService> logger)
    {
        _postRepository = postRepository;
        _logger = logger;
    }

    public async Task<Result<bool>> CreateAsync(Post post)
    {
        _logger.LogInformation("CreateAsync called with Post: {@Post}", post);

        if (post == null)
        {
            _logger.LogWarning("CreateAsync failed: Post is null.");
            return Result.Failure<bool>("Post cannot be null.");
        }

        await _postRepository.CreateAsync(post);
        _logger.LogInformation("Post created successfully: {@Post}", post);
        return Result.Success(true);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task<Result<Post>> GetByIdAsyncAggressiveInlining(string id)
    {
        _logger.LogInformation("GetByIdAsync called with Id: {Id}", id);

        var post = await _postRepository.GetByIdAsync(p => p.Id == id.ToString());
        if (post == null)
        {
            _logger.LogWarning("GetByIdAsync failed: Post not found for Id {Id}", id);
            return Result.Failure<Post>("Post not found.");
        }

        _logger.LogInformation("Post retrieved successfully: {@Post}", post);
        return Result.Success(post);
    }

    public async Task<Result<Post>> GetByIdAsync(string id)
    {
        _logger.LogInformation("GetByIdAsync called with Id: {Id}", id);

        var post = await _postRepository.GetByIdAsync(p => p.Id == id.ToString());
        if (post == null)
        {
            _logger.LogWarning("GetByIdAsync failed: Post not found for Id {Id}", id);
            return Result.Failure<Post>("Post not found.");
        }

        _logger.LogInformation("Post retrieved successfully: {@Post}", post);
        return Result.Success(post);
    }

    public async Task<Result<List<Post>>> GetAllAsync()
    {
        _logger.LogInformation("GetAllAsync called.");

        var posts = await _postRepository.GetAllAsync();
        _logger.LogInformation("Retrieved {Count} posts.", posts.Count);
        return Result.Success(posts);
    }

    public async Task<Result<bool>> UpdateAsync(Post post)
    {
        _logger.LogInformation("UpdateAsync called with Post: {@Post}", post);

        if (post == null)
        {
            _logger.LogWarning("UpdateAsync failed: Post is null.");
            return Result.Failure<bool>("Post cannot be null.");
        }

        var existing = await _postRepository.GetByIdAsync(p => p.Id == post.Id);
        if (existing == null)
        {
            _logger.LogWarning("UpdateAsync failed: Post not found for Id {Id}", post.Id);
            return Result.Failure<bool>("Post not found.");
        }

        await _postRepository.UpdateAsync(p => p.Id == post.Id, post);
        _logger.LogInformation("Post updated successfully: {@Post}", post);
        return Result.Success(true);
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeleteAsync called with Id: {Id}", id);

        var post = await _postRepository.GetByIdAsync(p => p.Id == id.ToString());
        if (post == null)
        {
            _logger.LogWarning("DeleteAsync failed: Post not found for Id {Id}", id);
            return Result.Failure<bool>("Post not found.");
        }

        await _postRepository.DeleteAsync(p => p.Id == id.ToString());
        _logger.LogInformation("Post deleted successfully for Id: {Id}", id);
        return Result.Success(true);
    }
}
