using FluentValidation;
using HelpDesk.Api.Models;

namespace HelpDesk.Api.Validators;

/// <summary>
/// Validador para CreateTicketRequest
/// </summary>
public class CreateTicketRequestValidator : AbstractValidator<CreateTicketRequest>
{
    public CreateTicketRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es requerido")
            .MinimumLength(5).WithMessage("El título debe tener al menos 5 caracteres")
            .MaximumLength(200).WithMessage("El título no puede exceder 200 caracteres");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción es requerida")
            .MinimumLength(10).WithMessage("La descripción debe tener al menos 10 caracteres")
            .MaximumLength(2000).WithMessage("La descripción no puede exceder 2000 caracteres");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("La prioridad debe ser 0 (Baja), 1 (Media) o 2 (Alta)");

        RuleFor(x => x.AssignedToUserId)
            .GreaterThan(0).WithMessage("El ID del usuario debe ser mayor que 0")
            .When(x => x.AssignedToUserId.HasValue);
    }
}
