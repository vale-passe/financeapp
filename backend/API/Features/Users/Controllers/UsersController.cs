using API.Features.Users.Entities;
using API.Infrastructure.Data;
using API.Shared.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Users.Controllers;

public class UsersController(DataContext context) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<User>>> GetUsers() =>
        await context.Users.ToListAsync();

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        if (await context.Users.FirstOrDefaultAsync(u => u.Id == id) is not { } user)
            return NotFound();

        return user;
    }
}