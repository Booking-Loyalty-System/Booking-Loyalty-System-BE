using Application.DTOs.Auth;
using FluentValidation;

namespace Application.Validators;

public class VerifyOtpRequestValidator : AbstractValidator<VerifyOtpRequest>
{
    public VerifyOtpRequestValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^(0|\+84)[0-9]{9}$").WithMessage("Phone number format is invalid.");

        RuleFor(x => x.OtpCode)
            .NotEmpty().WithMessage("OTP code is required.")
            .Length(6).WithMessage("OTP code must be exactly 6 digits.")
            .Matches(@"^[0-9]{6}$").WithMessage("OTP code must contain numbers only.");
    }
}