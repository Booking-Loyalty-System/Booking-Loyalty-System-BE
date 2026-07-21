using Application.DTOs.Reward;
using FluentValidation;

namespace Application.Validators;

public class UpdateRewardRequestValidator : AbstractValidator<UpdateRewardRequest>
{
    public UpdateRewardRequestValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Reward name must not exceed 100 characters.")
            .When(x => x.Name != null);

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => x.Description != null);

        RuleFor(x => x.PointsCost)
            .GreaterThan(0).WithMessage("Points cost must be greater than zero.")
            .When(x => x.PointsCost.HasValue);
    }
}
