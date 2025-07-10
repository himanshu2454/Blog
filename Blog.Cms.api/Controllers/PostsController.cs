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

    public PostsController(IPostService postService, IMapper mapper)
    {
        _postService = postService;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Policy = "RequireLoggedInUserRole")]
    public async Task<ActionResult<IEnumerable<Post>>> GetAll()
    {
        var result = await _postService.GetAllAsync();
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetById(string id)
    {
        var result = await _postService.GetByIdAsync(id);
        if (result.IsFailure || result.Value == null)
            return NotFound();
        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Post), StatusCodes.Status201Created)]
    public async Task<ActionResult<Post>> Create(PostRequest request)
    {
        var post = _mapper.Map<Post>(request);
        var created = await _postService.CreateAsync(post);
        if (created.IsFailure)
            return BadRequest();

        return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Post post)
    {
        if (id != post.Id)
            return BadRequest();

        var result = await _postService.UpdateAsync(post);
        if (result.IsFailure)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (!Guid.TryParse(id, out var guidId))
            return BadRequest("Invalid GUID format.");

        var result = await _postService.DeleteAsync(guidId);
        if (result.IsFailure)
            return NotFound();

        return NoContent();
    }
}
