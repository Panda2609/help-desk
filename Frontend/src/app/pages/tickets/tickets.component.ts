import { Component, OnInit, ViewChild, ElementRef, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TicketService } from '../../services/ticket.service';
import { AuthService } from '../../services/auth.service';
import { Ticket, CreateTicketRequest, UpdateTicketRequest } from '../../models/ticket.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tickets',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css']
})
export class TicketsComponent implements OnInit {
  @ViewChild('ticketModal') ticketModal!: ElementRef;
  
  tickets: Ticket[] = [];
  filteredTickets: Ticket[] = [];
  loading = false;
  error = '';
  page = 1;
  pageSize = 5;
  totalTickets = 0;
  Math = Math;  // Para usar en template
  
  editingTicketId: number | null = null;
  ticketForm: FormGroup;
  showModal = false;  // Control del modal

  statusOptions = [
    { value: 0, label: 'Nuevo' },
    { value: 1, label: 'En Progreso' },
    { value: 2, label: 'Resuelto' }
  ];

  priorityOptions = [
    { value: 0, label: 'Baja' },
    { value: 1, label: 'Media' },
    { value: 2, label: 'Alta' }
  ];

  filterStatus: number | null = null;
  filterPriority: number | null = null;

  constructor(
    private ticketService: TicketService,
    public authService: AuthService,
    private router: Router,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef
  ) {
    this.ticketForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(200)]],
      description: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(2000)]],
      priority: [0, Validators.required],
      status: [0, Validators.required],
      notes: ['']
    });
  }

  ngOnInit(): void {
    this.loadTickets();
  }

  loadTickets(): void {
    this.loading = true;
    this.error = '';

    const statusParam = this.filterStatus !== null ? this.filterStatus : undefined;
    const priorityParam = this.filterPriority !== null ? this.filterPriority : undefined;

    this.ticketService.getTickets(
      this.page, 
      this.pageSize, 
      statusParam,
      priorityParam
    )
      .subscribe({
        next: (response) => {
          this.tickets = response.items;
          this.filteredTickets = response.items;
          this.totalTickets = response.total;
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error('Error loading tickets:', err);
          this.error = 'Failed to load tickets';
          this.loading = false;
          this.cdr.detectChanges();
        }
      });
  }

  onFilterChange(): void {
    this.page = 1;
    this.loadTickets();
  }

  onCreateClick(): void {
    this.editingTicketId = null;
    this.ticketForm.reset({ status: 0, priority: 0 });
    this.showModal = true;
  }

  onEditClick(ticket: Ticket): void {
    this.editingTicketId = ticket.id;
    this.ticketForm.patchValue({
      title: ticket.title,
      description: ticket.description,
      priority: ticket.priority,
      status: ticket.status,
      notes: ticket.notes
    });
    this.showModal = true;
  }

  onDeleteClick(id: number): void {
    if (confirm('Are you sure you want to delete this ticket?')) {
      this.ticketService.deleteTicket(id).subscribe({
        next: () => {
          this.loadTickets();
        },
        error: (err) => {
          this.error = 'Failed to delete ticket';
        }
      });
    }
  }

  onSubmit(): void {
    if (this.ticketForm.invalid) {
      return;
    }

    const formValue = this.ticketForm.value;

    if (this.editingTicketId) {
      const updateRequest: UpdateTicketRequest = {
        title: formValue.title,
        description: formValue.description,
        priority: formValue.priority,
        status: formValue.status,
        notes: formValue.notes
      };

      this.ticketService.updateTicket(this.editingTicketId, updateRequest).subscribe({
        next: () => {
          this.showModal = false;
          this.editingTicketId = null;
          this.ticketForm.reset({ status: 0, priority: 0 });
          this.loadTickets();
        },
        error: (err) => {
          console.error('Error updating ticket:', err);
          this.error = 'Failed to update ticket: ' + (err.message || err.statusText || 'Unknown error');
        }
      });
    } else {
      const createRequest: CreateTicketRequest = {
        title: formValue.title,
        description: formValue.description,
        priority: formValue.priority
      };

      this.ticketService.createTicket(createRequest).subscribe({
        next: () => {
          this.showModal = false;
          this.editingTicketId = null;
          this.ticketForm.reset({ status: 0, priority: 0 });
          this.loadTickets();
        },
        error: (err) => {
          console.error('Error creating ticket:', err);
          this.error = 'Failed to create ticket: ' + (err.message || err.statusText || 'Unknown error');
        }
      });
    }
  }

  onCancel(): void {
    this.showModal = false;
    this.editingTicketId = null;
    this.ticketForm.reset({ status: 0, priority: 0 });
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  nextPage(): void {
    if (this.page * this.pageSize < this.totalTickets) {
      this.page++;
      this.loadTickets();
    }
  }

  previousPage(): void {
    if (this.page > 1) {
      this.page--;
      this.loadTickets();
    }
  }

  get f() {
    return this.ticketForm.controls;
  }

  getStatusLabel(status: number): string {
    return this.statusOptions.find(s => s.value === status)?.label || 'Unknown';
  }

  getPriorityLabel(priority: number): string {
    return this.priorityOptions.find(p => p.value === priority)?.label || 'Unknown';
  }
}
