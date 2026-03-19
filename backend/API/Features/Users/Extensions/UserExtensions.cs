using API.Features.Auth.Models;
using API.Features.Auth.Services.Interfaces;
using API.Features.Users.Entities;

namespace API.Features.Users.Extensions;

public static class UserExtensions
{
    public static UserTokenModel ToUserTokenModel(this User user, ITokenService tokenService) =>
        new()
        {
            Id = user.Id,
            Email = user.Email,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            AvatarImageUrl = user.ImageAvatarUrl,
            Token = tokenService.CreateToken(user)
        };
}