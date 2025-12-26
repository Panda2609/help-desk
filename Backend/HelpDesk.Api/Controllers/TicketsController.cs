using HelpDesk.Api.Models;
using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;
using HelpDesk.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IRepository<User> _userRepository;
    private readonly ILogger<TicketsController> _logger;

    public TicketsController(ITicketRepository ticketRepository, IRepository<User> userRepository, ILogger<TicketsController> logger)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetTickets([FromQuery] int page = 1, [FromQuery] int pageSize = 10, 
        [FromQuery] int? status = null, [FromQuery] int? priority = null)
    {
        var statusEnum = status.HasValue ? (TicketStatus?)status.Value : null;
        var priorityEnum = priority.HasValue ? (Priority?)priority.Value : null;
        
        var (tickets, total) = await _ticketRepository.GetTicketsPagedAsync(page, pageSize, statusEnum, priorityEnum);
        var dtos = tickets.Select(MapToDto).ToList();

        return Ok(new { items = dtos, total, page, pageSize });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicket(int id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null)
            return NotFound(new { message = "Ticket no encontrado" });

        return Ok(MapToDto(ticket));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] CreateTicketRequest request)
    {
        var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
        
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return Unauthorized(new { message = "Usuario no válido" });

        var ticket = new Ticket
        {
            Title = request.Title,
            Description = request.Description,
            Priority = (Priority)request.Priority,
            Status = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedByUserId = userId,
            AssignedToUserId = request.AssignedToUserId
        };

        await _ticketRepository.AddAsync(ticket);
        await _ticketRepository.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, MapToDto(ticket));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicket(int id, [FromBody] UpdateTicketRequest request)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null)
            return NotFound(new { message = "Ticket no encontrado" });

        if (!string.IsNullOrEmpty(request.Title))
            ticket.Title = request.Title;
        if (!string.IsNullOrEmpty(request.Description))
            ticket.Description = request.Description;
        if (request.Priority.HasValue)
            ticket.Priority = (Priority)request.Priority.Value;
        if (request.Status.HasValue)
        {
            ticket.Status = (TicketStatus)request.Status.Value;
            if (request.Status.Value == 2)
                ticket.ResolvedAt = DateTime.UtcNow;
        }
        if (request.AssignedToUserId.HasValue)
            ticket.AssignedToUserId = request.AssignedToUserId.Value;
        if (!string.IsNullOrEmpty(request.Notes))
            ticket.Notes = request.Notes;

        ticket.UpdatedAt = DateTime.UtcNow;

        await _ticketRepository.UpdateAsync(ticket);
        await _ticketRepository.SaveChangesAsync();

        return Ok(MapToDto(ticket));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null)
            return NotFound(new { message = "Ticket no encontrado" });

        await _ticketRepository.DeleteAsync(ticket);
        await _ticketRepository.SaveChangesAsync();

        return NoContent();
    }

    private TicketDto MapToDto(Ticket ticket)
    {
        return new TicketDto
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            Priority = (int)ticket.Priority,
            Status = (int)ticket.Status,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt,
            ResolvedAt = ticket.ResolvedAt,
            CreatedByUser = new UserDto
            {
                Id = ticket.CreatedByUser!.Id,
                Username = ticket.CreatedByUser.Username,
                FullName = ticket.CreatedByUser.FullName,
                Email = ticket.CreatedByUser.Email
            },
            AssignedToUser = ticket.AssignedToUser != null ? new UserDto
            {
                Id = ticket.AssignedToUser.Id,
                Username = ticket.AssignedToUser.Username,
                FullName = ticket.AssignedToUser.FullName,
                Email = ticket.AssignedToUser.Email
            } : null,
            Notes = ticket.Notes
        };
    }
}
