using Blog.Domain.Enums;
using Blog.Persistance.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Blog.Domain.Entities;

public class Post : BaseEntity
{
    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement("content")]
    public string Content { get; set; } = string.Empty;

    [BsonElement("authorId")]
    [BsonRepresentation(BsonType.String)]
    public Guid AuthorId { get; set; } = Guid.NewGuid();

    [BsonElement("tags")]
    public List<string> Tags { get; set; } = new List<string>();

    [BsonElement("category")]
    public Categories Category { get; set; } = Categories.None;

    [BsonElement("status")]
    public Status Status { get; set; } = Status.Draft;

    [BsonElement("CreatedAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
