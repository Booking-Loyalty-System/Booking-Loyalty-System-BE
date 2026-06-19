using Application.DTOs.Reward;
using FluentValidation;

namespace Application.Validators;

public class CreateRewardRequestValidator : AbstractValidator<CreateRewardRequest>
{
    public CreateRewardRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Reward name is required.")
            .MaximumLength(100).WithMessage("Reward name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.PointsCost)
            .GreaterThan(0).WithMessage("Points cost must be greater than zero.");
    }
}
