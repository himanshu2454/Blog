using Blog.Domain.Enums;
using Blog.Persistance.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Blog.Domain.Entities;

public class User : BaseEntity
{
    [BsonElement("username")]
    public string Username { get; set; } = string.Empty;

    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("role")]
    public UserRole[] Role { get; set; } = [UserRole.Viewer];

    [BsonElement("scopes")]
    public string[] Scopes { get; set; } = [];

    [BsonElement("bio")]
    public string Bio { get; set; } = string.Empty;

    [BsonElement("profileImageUrl")]
    public string ProfileImageUrl { get; set; } = string.Empty;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}