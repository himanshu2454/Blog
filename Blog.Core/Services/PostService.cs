using Blog.Core.Repository;
using Blog.Domain.Entities;
using CSharpFunctionalExtensions;

namespace Blog.Core.Services;

public interface IPostService
{
    Task<Result<bool>> CreateAsync(Post post);
    Task<Result<Post>> GetByIdAsync(string id);
    Task<Result<List<Post>>> GetAllAsync();
    Task<Result<bool>> UpdateAsync(Post post);
    Task<Result<bool>> DeleteAsync(Guid id);
}

public class PostService : IPostService
{
    private readonly IPostRepo _postRepository;

    public PostService(IPostRepo postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<Result<bool>> CreateAsync(Post post)
    {
        if (post == null)
            return Result.Failure<bool>("Post cannot be null.");

        await _postRepository.CreateAsync(post);
        return Result.Success(true);
    }

    public async Task<Result<Post>> GetByIdAsync(string id)
    {
        var post = await _postRepository.GetByIdAsync(p => p.Id == id.ToString());
        if (post == null)
            return Result.Failure<Post>("Post not found.");

        return Result.Success(post);
    }

    public async Task<Result<List<Post>>> GetAllAsync()
    {
        var posts = await _postRepository.GetAllAsync();
        return Result.Success(posts);
    }

    public async Task<Result<bool>> UpdateAsync(Post post)
    {
        if (post == null)
            return Result.Failure<bool>("Post cannot be null.");

        var existing = await _postRepository.GetByIdAsync(p => p.Id == post.Id);
        if (existing == null)
            return Result.Failure<bool>("Post not found.");

        await _postRepository.UpdateAsync(p => p.Id == post.Id, post);
        return Result.Success(true);
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        var post = await _postRepository.GetByIdAsync(p => p.Id == id.ToString());
        if (post == null)
            return Result.Failure<bool>("Post not found.");

        await _postRepository.DeleteAsync(p => p.Id == id.ToString());
        return Result.Success(true);
    }
}
