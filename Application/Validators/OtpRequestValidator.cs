using Application.DTOs.Auth;
using FluentValidation;

namespace Application.Validators;

public class OtpRequestValidator : AbstractValidator<OtpRequest>
{
    public OtpRequestValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Length(10, 12).WithMessage("Phone number length must be between 10 and 12 characters.")
            .Matches(@"^(0|\+84)[0-9]{9}$").WithMessage("Phone number must match format: 0xxxxxxxxx or +84xxxxxxxxx.");
    }
}