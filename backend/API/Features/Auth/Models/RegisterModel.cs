namespace API.Features.Auth.Models;

public record RegisterModel(
    string Email,
    string Firstname,
    string Lastname,
    string Password,
    string ImageAvatarUrl
);