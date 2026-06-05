using Application.DTOs.Customer;
using FluentValidation;

namespace Application.Validators;

public class UpdateCustomerProfileRequestValidator : AbstractValidator<UpdateCustomerProfileRequest>
{
    public UpdateCustomerProfileRequestValidator()
    {
        RuleFor(x => x.FullName)
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters.")
            .When(x => x.FullName != null);

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^[0-9]{9,11}$").WithMessage("Phone number must be 9-11 digits.")
            .When(x => x.PhoneNumber != null);
    }
}
