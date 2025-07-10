using AutoMapper;
using Blog.Cms.Api.Models;
using Blog.Core.Services;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Cms.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        var result = await _userService.GetAllUsersAsync();
        if (result.IsFailure || result.Value == null)
            return NotFound();
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(string id)
    {
        var result = await _userService.GetUserByIdAsync(id);
        if (result.IsFailure || result.Value == null)
            return NotFound();
        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    public async Task<ActionResult<User>> Create(CreateUserRequest request)
    {
        var user = _mapper.Map<User>(request);
        var created = await _userService.CreateUserAsync(user);
        if (created.IsFailure)
            return BadRequest();

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, User user)
    {
        if (id != user.Id)
            return BadRequest();

        var result = await _userService.UpdateUserAsync(id, user);
        if (result.IsFailure)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (result.IsFailure)
            return NotFound();

        return NoContent();
    }
}
