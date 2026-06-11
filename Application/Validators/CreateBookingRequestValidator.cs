using Application.DTOs.Booking;
using FluentValidation;

namespace Application.Validators;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch is required.");

        RuleFor(x => x.VehicleId)
            .NotEmpty().WithMessage("Vehicle is required.");

        RuleFor(x => x.WashPackageId)
            .NotEmpty().WithMessage("Wash package is required.");

        RuleFor(x => x.BookingDate)
            .NotEmpty().WithMessage("Booking date is required.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required.");
    }
}
