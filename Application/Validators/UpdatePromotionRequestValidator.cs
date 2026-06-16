using Application.DTOs.Promotion;
using Domain.Enums;
using FluentValidation;

namespace Application.Validators;

public class UpdatePromotionRequestValidator : AbstractValidator<UpdatePromotionRequest>
{
    public UpdatePromotionRequestValidator()
    {
        RuleFor(x => x.DiscountType)
            .Must(v => Enum.TryParse<DiscountType>(v, out _))
            .When(x => x.DiscountType != null)
            .WithMessage("Discount type must be 'Percentage' or 'FixedAmount'.");

        RuleFor(x => x.DiscountValue)
            .GreaterThan(0).When(x => x.DiscountValue.HasValue)
            .WithMessage("Discount value must be greater than zero.");

        RuleFor(x => x.MaxUses)
            .GreaterThan(0).When(x => x.MaxUses.HasValue)
            .WithMessage("Max uses must be greater than zero.");

        RuleFor(x => x.MinSpend)
            .GreaterThanOrEqualTo(0).When(x => x.MinSpend.HasValue)
            .WithMessage("Minimum spend cannot be negative.");
    }
}
