using FluentValidation;

namespace Products.WebAPI.Features.Sales.CheckoutSale;

public class CheckoutSaleValidator : AbstractValidator<CheckoutSaleCommand>
{
    public  CheckoutSaleValidator()
    {
        RuleFor(r => r.UserId).GreaterThan(0);
        
        RuleFor(r => r.Items).NotEmpty()
            .WithMessage("A venda precisa ter pelo menos um item.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.Barcode).NotEmpty();
            item.RuleFor(i => i.Quantity).GreaterThan(0);
        });

        RuleFor(x => x.PaymentMethod)
            .NotNull().WithMessage("Informe a forma de pagamento.")
            .IsInEnum().WithMessage("Forma de pagamento inválida.");
    }
}