using FluentValidation;
using HelpDesk.Api.Models;

namespace HelpDesk.Api.Validators;

/// <summary>
/// Validador para UpdateTicketRequest
/// </summary>
public class UpdateTicketRequestValidator : AbstractValidator<UpdateTicketRequest>
{
    public UpdateTicketRequestValidator()
    {
        RuleFor(x => x.Title)
            .MinimumLength(5).WithMessage("El título debe tener al menos 5 caracteres")
            .MaximumLength(200).WithMessage("El título no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Description)
            .MinimumLength(10).WithMessage("La descripción debe tener al menos 10 caracteres")
            .MaximumLength(2000).WithMessage("La descripción no puede exceder 2000 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("La prioridad debe ser 0 (Baja), 1 (Media) o 2 (Alta)")
            .When(x => x.Priority.HasValue);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("El estado debe ser 0 (Nuevo), 1 (EnProgreso) o 2 (Resuelto)")
            .When(x => x.Status.HasValue);

        RuleFor(x => x.AssignedToUserId)
            .GreaterThan(0).WithMessage("El ID del usuario debe ser mayor que 0")
            .When(x => x.AssignedToUserId.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Las notas no pueden exceder 2000 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
