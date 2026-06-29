using Application.DTOs.Promotion;
using Domain.Enums;
using FluentValidation;

namespace Application.Validators;

public class CreatePromotionRequestValidator : AbstractValidator<CreatePromotionRequest>
{
    public CreatePromotionRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Promotion code is required.")
            .MaximumLength(30).WithMessage("Promotion code must not exceed 30 characters.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Promotion name is required.")
            .MaximumLength(255).WithMessage("Promotion name must not exceed 255 characters.");

        RuleFor(x => x.DiscountType)
            .NotEmpty().WithMessage("Discount type is required.")
            .Must(v => Enum.TryParse<DiscountType>(v, out _))
            .WithMessage("Discount type must be 'Percentage' or 'FixedAmount'.");

        RuleFor(x => x.DiscountValue)
            .GreaterThan(0).WithMessage("Discount value must be greater than zero.");

        RuleFor(x => x.DiscountValue)
            .LessThanOrEqualTo(100)
            .When(x => string.Equals(x.DiscountType, nameof(DiscountType.Percentage), StringComparison.OrdinalIgnoreCase))
            .WithMessage("Percentage discount cannot exceed 100.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");

        RuleFor(x => x.MaxUses)
            .GreaterThan(0).When(x => x.MaxUses.HasValue)
            .WithMessage("Max uses must be greater than zero.");

        RuleFor(x => x.MinSpend)
            .GreaterThanOrEqualTo(0).When(x => x.MinSpend.HasValue)
            .WithMessage("Minimum spend cannot be negative.");
    }
}
