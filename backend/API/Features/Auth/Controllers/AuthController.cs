using System.Security.Cryptography;
using System.Text;
using API.Features.Auth.Models;
using API.Features.Auth.Services.Interfaces;
using API.Features.Users.Entities;
using API.Features.Users.Extensions;
using API.Infrastructure.Data;
using API.Shared.Controllers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Auth.Controllers;

public class AuthController(
    DataContext context,
    ITokenService tokenService,
    IValidator<RegisterModel> registerModelValidator,
    IValidator<LoginModel> loginModelValidator) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserTokenModel>> Register([FromBody] RegisterModel model)
    {
        var validationResult = await registerModelValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        using var hmac = new HMACSHA512();

        var user = new User
        {
            Email = model.Email.Trim().ToLower(),
            Firstname = model.Firstname.Trim(),
            Lastname = model.Lastname.Trim(),
            ImageAvatarUrl = model.ImageAvatarUrl,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user.ToUserTokenModel(tokenService);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserTokenModel>> Login([FromBody] LoginModel model)
    {
        var validationResult = await loginModelValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var normalizedEmail = model.Email.Trim().ToLower();
        if (await context.Users.SingleOrDefaultAsync(u => u.Email == normalizedEmail) is not { } user)
            return Unauthorized("Invalid email or password.");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password));

        if (!computedHash.SequenceEqual(user.PasswordHash))
            return Unauthorized("Invalid email or password.");

        return user.ToUserTokenModel(tokenService);
    }
}