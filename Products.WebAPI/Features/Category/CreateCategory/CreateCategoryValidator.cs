using FluentValidation;

namespace Products.WebAPI.Features.Category.CreateCategory;

public class CreateCategoryValidator :  AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Nome não pode ser nulo")
            .MaximumLength(30);
    }
}