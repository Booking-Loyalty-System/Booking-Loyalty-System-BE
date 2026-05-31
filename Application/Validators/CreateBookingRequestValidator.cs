using Application.DTOs.Booking;
using FluentValidation;

namespace Application.Validators;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.VehicleId)
            .NotEmpty().WithMessage("Vehicle is required.");

        RuleFor(x => x.ServiceId)
            .NotEmpty().WithMessage("Service is required.");

        RuleFor(x => x.StoreId)
            .NotEmpty().WithMessage("Store is required.");

        RuleFor(x => x.BookingDate)
            .NotEmpty().WithMessage("Booking date is required.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required.");
    }
}
