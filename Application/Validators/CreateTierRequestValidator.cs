using Application.DTOs.Tier;
using Domain.Enums;
using FluentValidation;

namespace Application.Validators;

public class CreateTierRequestValidator : AbstractValidator<CreateTierRequest>
{
    public CreateTierRequestValidator()
    {
        RuleFor(x => x.TierName)
            .NotEmpty().WithMessage("Tier name is required.")
            .MaximumLength(50).WithMessage("Tier name must not exceed 50 characters.");

        RuleFor(x => x.PointRate)
            .GreaterThan(0).WithMessage("Point rate must be greater than 0.");

        RuleFor(x => x.BookingWindow)
            .GreaterThan(0).WithMessage("Booking window must be greater than 0.");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Level is required.")
            .Must(l => Enum.TryParse<PriorityLevel>(l, true, out _))
            .WithMessage("Level must be a valid priority level (Bronze, Silver, Gold, Diamond).");

        RuleFor(x => x.MinPointsRequired)
            .GreaterThanOrEqualTo(0).WithMessage("Min points required must be >= 0.");

        RuleFor(x => x.MaintenancePoints)
            .GreaterThanOrEqualTo(0).WithMessage("Maintenance points must be >= 0.");
    }
}
