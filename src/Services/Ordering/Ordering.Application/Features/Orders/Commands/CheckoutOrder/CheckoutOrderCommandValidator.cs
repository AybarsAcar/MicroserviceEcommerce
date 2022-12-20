using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder;

/// <summary>
/// This will run in the CheckoutOrderCommand when we receive a CheckoutOrderCommand
/// So handle method will not be called unless the validation passes
/// </summary>
public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
{
  public CheckoutOrderCommandValidator()
  {
    RuleFor(c => c.Username)
      .NotEmpty().WithMessage("{Username} is required")
      .NotNull()
      .MaximumLength(50).WithMessage("{Username} must not exceed 50 characters.");

    RuleFor(c => c.EmailAddress)
      .NotEmpty().WithMessage("{Email} is required");

    RuleFor(c => c.TotalPrice)
      .NotEmpty().WithMessage("{TotalPrice} is required")
      .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero.");
  }
}