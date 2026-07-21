using Application.DTOs.AddOn;
using FluentValidation;

namespace Application.Validators;

public class CreateAddOnRequestValidator : AbstractValidator<CreateAddOnRequest>
{
    public CreateAddOnRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Add-on name is required.")
            .MaximumLength(100).WithMessage("Add-on name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.DurationMinutes)
            .GreaterThanOrEqualTo(0).WithMessage("Duration minutes cannot be negative.");
    }
}
