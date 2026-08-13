using FluentValidation;

namespace Products.WebAPI.Features.Brand.CreateBrand;

public class CreateBrandValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Nome não pode ser nulo")
            .MaximumLength(30);
    }
}