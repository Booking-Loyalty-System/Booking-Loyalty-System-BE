using Application.DTOs.Vehicle;
using FluentValidation;

namespace Application.Validators;

public class AddVehicleRequestValidator : AbstractValidator<AddVehicleRequest>
{
    public AddVehicleRequestValidator()
    {
        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("License plate is required.")
            .MaximumLength(20).WithMessage("License plate must not exceed 20 characters.");

        RuleFor(x => x.VehicleType)
            .NotEmpty().WithMessage("Vehicle type is required.")
            .Must(t => t == "Small" || t == "Medium" || t == "Large")
            .WithMessage("Vehicle type must be Small, Medium, or Large.");
    }
}
