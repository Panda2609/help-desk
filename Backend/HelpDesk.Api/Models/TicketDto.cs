namespace HelpDesk.Api.Models;

public class TicketDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int Priority { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public required UserDto CreatedByUser { get; set; }
    public UserDto? AssignedToUser { get; set; }
    public string? Notes { get; set; }
}

public class CreateTicketRequest
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int Priority { get; set; }
    public int? AssignedToUserId { get; set; }
}

public class UpdateTicketRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? Priority { get; set; }
    public int? Status { get; set; }
    public int? AssignedToUserId { get; set; }
    public string? Notes { get; set; }
}
