using Blog.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;
namespace Blog.Cms.Api.Models;

public class CreateUserRequest
{
    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Viewer;

    public string Bio { get; set; } = string.Empty;

    public string ProfileImageUrl { get; set; } = string.Empty;
}
