using Application.DTOs.Auth;
using FluentValidation;

namespace Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Length(10, 12).WithMessage("Phone number length must be between 10 and 12 characters.")
            .Matches(@"^(0|\+84)[0-9]{9}$").WithMessage("Phone number must match format: 0xxxxxxxxx or +84xxxxxxxxx.");

        RuleFor(x => x.VehicleType)
            .IsInEnum().WithMessage("Invalid vehicle type.");
    }
}
