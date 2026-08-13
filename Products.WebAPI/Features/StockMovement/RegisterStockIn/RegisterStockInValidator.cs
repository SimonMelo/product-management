using FluentValidation;
using Products.WebAPI.Features.StockMovement.RegisterStockIn;

namespace Products.WebAPI.Features.StockMovement.RegisterStockIn;

public class RegisterStockInValidator : AbstractValidator<RegisterStockInCommand>
{
    public RegisterStockInValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Informe ao menos um produto para dar entrada.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.Barcode).NotEmpty();
            item.RuleFor(i => i.Quantity).GreaterThan(0);
        });
    }
}