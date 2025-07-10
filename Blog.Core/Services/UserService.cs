using Blog.Core.Repository;
using Blog.Domain.Entities;
using CSharpFunctionalExtensions;

namespace Blog.Core.Services;

public interface IUserService
{
    Task<Result<List<User>>> GetAllUsersAsync();
    Task<Result<User>> GetUserByIdAsync(string id);
    Task<Result> CreateUserAsync(User user);
    Task<Result> UpdateUserAsync(string id, User user);
    Task<Result> DeleteUserAsync(string id);
}

public class UserService : IUserService
{
    private readonly IUserRepo _userRepo;

    public UserService(IUserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<Result<List<User>>> GetAllUsersAsync()
    {
        var users = await _userRepo.GetAllAsync();
        if (users is null)
            return Result.Failure<List<User>>("No users found.");
        return Result.Success(users);
    }

    public async Task<Result<User>> GetUserByIdAsync(string id)
    {
        var user = await _userRepo.GetByIdAsync(u => u.Id == id);
        if (user is null)
            return Result.Failure<User>("User not found.");
        return Result.Success(user);
    }

    public async Task<Result> CreateUserAsync(User user)
    {
        await _userRepo.CreateAsync(user);
        return Result.Success();
    }

    public async Task<Result> UpdateUserAsync(string id, User user)
    {
        await _userRepo.UpdateAsync(u => u.Id == id, user);
        return Result.Success();
    }

    public async Task<Result> DeleteUserAsync(string id)
    {
        await _userRepo.DeleteAsync(u => u.Id == id);
        return Result.Success();
    }
}