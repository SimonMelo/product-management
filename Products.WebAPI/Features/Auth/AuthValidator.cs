using FluentValidation;

namespace Products.WebAPI.Features.Auth;

public class AuthValidator : AbstractValidator<AuthCommand>
{
    public AuthValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail informado não é válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.");
    }
}