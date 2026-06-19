using Application.DTOs.Tier;
using Domain.Enums;
using FluentValidation;

namespace Application.Validators;

public class UpdateTierRequestValidator : AbstractValidator<UpdateTierRequest>
{
    public UpdateTierRequestValidator()
    {
        RuleFor(x => x.TierName)
            .MaximumLength(50).WithMessage("Tier name must not exceed 50 characters.")
            .When(x => x.TierName != null);

        RuleFor(x => x.PointRate)
            .GreaterThan(0).WithMessage("Point rate must be greater than 0.")
            .When(x => x.PointRate != null);

        RuleFor(x => x.BookingWindow)
            .GreaterThan(0).WithMessage("Booking window must be greater than 0.")
            .When(x => x.BookingWindow != null);

        RuleFor(x => x.Level)
            .Must(l => Enum.TryParse<PriorityLevel>(l, true, out _))
            .WithMessage("Level must be a valid priority level (Bronze, Silver, Gold, Diamond).")
            .When(x => x.Level != null);

        RuleFor(x => x.MinPointsRequired)
            .GreaterThanOrEqualTo(0).WithMessage("Min points required must be >= 0.")
            .When(x => x.MinPointsRequired != null);

        RuleFor(x => x.MaintenancePoints)
            .GreaterThanOrEqualTo(0).WithMessage("Maintenance points must be >= 0.")
            .When(x => x.MaintenancePoints != null);
    }
}
