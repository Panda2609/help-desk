using FluentValidation;
using HelpDesk.Api.Models;

namespace HelpDesk.Api.Validators;

/// <summary>
/// Validador para LoginRequest
/// </summary>
public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario es requerido")
            .MinimumLength(3).WithMessage("El nombre de usuario debe tener al menos 3 caracteres")
            .MaximumLength(100).WithMessage("El nombre de usuario no puede exceder 100 caracteres");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida")
            .MinimumLength(4).WithMessage("La contraseña debe tener al menos 4 caracteres")
            .MaximumLength(255).WithMessage("La contraseña no puede exceder 255 caracteres");
    }
}
