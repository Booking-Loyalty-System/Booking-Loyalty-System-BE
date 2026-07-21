using Application.DTOs.AddOn;
using FluentValidation;

namespace Application.Validators;

public class UpdateAddOnRequestValidator : AbstractValidator<UpdateAddOnRequest>
{
    public UpdateAddOnRequestValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Add-on name must not exceed 100 characters.")
            .When(x => x.Name != null);

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => x.Description != null);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.")
            .When(x => x.Price.HasValue);

        RuleFor(x => x.DurationMinutes)
            .GreaterThanOrEqualTo(0).WithMessage("Duration minutes cannot be negative.")
            .When(x => x.DurationMinutes.HasValue);
    }
}
