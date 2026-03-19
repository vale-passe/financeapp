namespace API.Features.Users.Entities;

public class User
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string Firstname { get; init; } =  string.Empty;
    public string Lastname { get; init; } =  string.Empty;
    public string ImageAvatarUrl { get; set; } = string.Empty;
    public byte[] PasswordHash { get; init; } = [];
    public byte[] PasswordSalt { get; init; } = [];
}