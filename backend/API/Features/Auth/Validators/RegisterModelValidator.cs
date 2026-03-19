using System.ComponentModel.DataAnnotations;
using API.Features.Auth.Models;
using API.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Auth.Validators;

public class RegisterModelValidator : AbstractValidator<RegisterModel>
{
    public RegisterModelValidator(DataContext context)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(128).WithMessage("Email maximum length is 128.")
            .EmailAddress().WithMessage("Invalid email address.")
            .MustAsync(async (email, ct) =>
            {
                var normalizedEmail = email.Trim().ToLower();
                return !await context.Users.AnyAsync(u => u.Email == normalizedEmail, ct);
            })
            .WithMessage("Email is already in use.");

        RuleFor(x => x.Firstname)
            .NotEmpty().WithMessage("Firstname is required.")
            .MaximumLength(64).WithMessage("Firstname maximum length is 64.");
        
        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage("Lastname is required.")
            .MaximumLength(64).WithMessage("Lastname maximum length is 64.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(4).WithMessage("Password minimum length is 4.");

        RuleFor(x => x.ImageAvatarUrl)
            .NotEmpty().WithMessage("ImageAvatarUrl is required.");
    }
}