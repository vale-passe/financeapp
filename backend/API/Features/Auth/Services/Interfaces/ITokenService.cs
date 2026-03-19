using API.Features.Users.Entities;

namespace API.Features.Auth.Services.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}