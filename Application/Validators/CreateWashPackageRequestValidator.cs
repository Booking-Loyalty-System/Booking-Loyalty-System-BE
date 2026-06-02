using Application.DTOs.WashPackage;
using FluentValidation;

namespace Application.Validators;

public class CreateWashPackageRequestValidator : AbstractValidator<CreateWashPackageRequest>
{
    public CreateWashPackageRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("Duration must be greater than 0.");

        RuleFor(x => x.VehicleType)
            .Must(t => t == null || t == "Small" || t == "Medium" || t == "Large")
            .WithMessage("Vehicle type must be Small, Medium, or Large.");
    }
}
