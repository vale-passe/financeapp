namespace API.Features.Auth.Models;

public class UserTokenModel
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string AvatarImageUrl { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
