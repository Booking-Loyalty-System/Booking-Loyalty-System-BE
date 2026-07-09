using Application.DTOs.Booking;
using FluentValidation;

namespace Application.Validators;

public class AddBookingImageRequestValidator : AbstractValidator<AddBookingImageRequest>
{
    public AddBookingImageRequestValidator()
    {
        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("Image URL is required.")
            .MaximumLength(1000).WithMessage("Image URL must not exceed 1000 characters.")
            .Must(BeAValidHttpsUrl).WithMessage("Image URL must be a valid https URL.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Image type is required.")
            .Must(t => t == "BeforeWash" || t == "AfterWash")
            .WithMessage("Image type must be BeforeWash or AfterWash.");

        RuleFor(x => x.Note)
            .MaximumLength(500).WithMessage("Note must not exceed 500 characters.");
    }

    private static bool BeAValidHttpsUrl(string? url)
        => Uri.TryCreate(url, UriKind.Absolute, out var uri) && uri.Scheme == Uri.UriSchemeHttps;
}
