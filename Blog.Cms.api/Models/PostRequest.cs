using Blog.Domain.Enums;

namespace Blog.Cms.Api.Models;

public class PostRequest
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public Guid AuthorId { get; set; } = default;

    public List<string> Tags { get; set; } = [];

    public Categories Category { get; set; } = Categories.None;

    public Status Status { get; set; } = Status.Draft;
}