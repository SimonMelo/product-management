using FluentValidation;

namespace Products.WebAPI.Features.StockMovement.RegisterAdjustment;

public class RegisterAdjustmentValidator : AbstractValidator<RegisterAdjustmentCommand>
{
    public RegisterAdjustmentValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.Barcode).NotEmpty();
        RuleFor(x => x.Quantity).NotEqual(0).WithMessage("Informe uma quantidade diferente de zero.");
    }
}
