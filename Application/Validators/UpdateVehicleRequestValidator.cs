using Application.DTOs.Vehicle;
using FluentValidation;

namespace Application.Validators;

public class UpdateVehicleRequestValidator : AbstractValidator<UpdateVehicleRequest>
{
    public UpdateVehicleRequestValidator()
    {
        RuleFor(x => x.LicensePlate)
            .MaximumLength(20).WithMessage("License plate must not exceed 20 characters.")
            .When(x => x.LicensePlate != null);

        RuleFor(x => x.VehicleType)
            .Must(t => t == "Small" || t == "Medium" || t == "Large")
            .WithMessage("Vehicle type must be Small, Medium, or Large.")
            .When(x => x.VehicleType != null);
    }
}
