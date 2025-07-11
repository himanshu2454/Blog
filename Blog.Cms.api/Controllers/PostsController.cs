using AutoMapper;
using Blog.Cms.Api.Models;
using Blog.Core.Services;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Cms.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IMapper _mapper;
    private readonly ILogger<PostsController> _logger;

    public PostsController(IPostService postService, IMapper mapper, ILogger<PostsController> logger)
    {
        _postService = postService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Policy = "RequireLoggedInUserRole")]
    public async Task<ActionResult<IEnumerable<Post>>> GetAll()
    {
        _logger.LogInformation($"Entered controller {nameof(PostsController)}");
        _logger.LogInformation($"Starting services {nameof(_postService.GetAllAsync)}");
        var result = await _postService.GetAllAsync();
        _logger.LogInformation($"Exiting services {nameof(_postService.GetAllAsync)}");
        _logger.LogInformation($"Result: {result.Value}");
        _logger.LogInformation($"Exiting controller {nameof(PostsController)}");
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetById(string id)
    {
        _logger.LogInformation($"Entered controller {nameof(PostsController)}.{nameof(GetById)} with id: {id}");
        _logger.LogInformation($"Starting services {nameof(_postService.GetByIdAsync)}");
        var result = await _postService.GetByIdAsync(id);
        _logger.LogInformation($"Exiting services {nameof(_postService.GetByIdAsync)}");
        if (result.IsFailure || result.Value == null)
        {
            _logger.LogWarning($"Post not found for id: {id}");
            _logger.LogInformation($"Exiting controller {nameof(PostsController)}.{nameof(GetById)}");
            return NotFound();
        }
        _logger.LogInformation($"Result: {result.Value}");
        _logger.LogInformation($"Exiting controller {nameof(PostsController)}.{nameof(GetById)}");
        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Post), StatusCodes.Status201Created)]
    public async Task<ActionResult<Post>> Create(PostRequest request)
    {
        _logger.LogInformation($"Entered controller {nameof(PostsController)}.{nameof(Create)}");
        var post = _mapper.Map<Post>(request);
        _logger.LogInformation($"Starting services {nameof(_postService.CreateAsync)}");
        var created = await _postService.CreateAsync(post);
        _logger.LogInformation($"Exiting services {nameof(_postService.CreateAsync)}");
        if (created.IsFailure)
        {
            _logger.LogWarning("Failed to create post.");
            _logger.LogInformation($"Exiting controller {nameof(PostsController)}.{nameof(Create)}");
            return BadRequest();
        }
        _logger.LogInformation($"Post created with id: {post.Id}");
        _logger.LogInformation($"Exiting controller {nameof(PostsController)}.{nameof(Create)}");
        return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Post post)
    {
        _logger.LogInformation($"Entered controller {nameof(PostsController)}.{nameof(Update)} with id: {id}");
        if (id != post.Id)
        {
            _logger.LogWarning("Id mismatch in update request.");
            _logger.LogInformation($"Exiting controller {nameof(PostsController)}.{nameof(Update)}");
            return BadRequest();
        }
        _logger.LogInformation($"Starting services {nameof(_postService.UpdateAsync)}");
        var result = await _postService.UpdateAsync(post);
        _logger.LogInformation($"Exiting services {nameof(_postService.UpdateAsync)}");
        if (result.IsFailure)
        {
            _logger.LogWarning($"Post not found for update with id: {id}");
            _logger.LogInformation($"Exiting controller {nameof(PostsController)}.{nameof(Update)}");
            return NotFound();
        }
        _logger.LogInformation($"Post updated with id: {id}");
        _logger.LogInformation($"Exiting controller {nameof(PostsController)}.{nameof(Update)}");
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        _logger.LogInformation($"Entered controller {nameof(PostsController)}.{nameof(Delete)} with id: {id}");
        if (!Guid.TryParse(id, out var guidId))
        {
            _logger.LogWarning("Invalid GUID format in delete request.");
            _logger.LogInformation($"Exiting controller {nameof(PostsController)}.{nameof(Delete)}");
            return BadRequest("Invalid GUID format.");
        }
        _logger.LogInformation($"Starting services {nameof(_postService.DeleteAsync)}");
        var result = await _postService.DeleteAsync(guidId);
        _logger.LogInformation($"Exiting services {nameof(_postService.DeleteAsync)}");
        if (result.IsFailure)
        {
            _logger.LogWarning($"Post not found for delete with id: {id}");
            _logger.LogInformation($"Exiting controller {nameof(PostsController)}.{nameof(Delete)}");
            return NotFound();
        }
        _logger.LogInformation($"Post deleted with id: {id}");
        _logger.LogInformation($"Exiting controller {nameof(PostsController)}.{nameof(Delete)}");
        return NoContent();
    }
}
