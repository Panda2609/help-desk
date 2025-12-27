import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TicketService } from './ticket.service';

describe('TicketService', () => {
  let service: TicketService;
  let httpMock: HttpTestingController;
  const API_URL = 'http://localhost:5264/api';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [TicketService]
    });
    service = TestBed.inject(TicketService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch tickets with default parameters', () => {
    const mockTickets = {
      items: [
        { id: 1, title: 'Test Ticket', description: 'Test Description', priority: 1, status: 0, createdAt: new Date() }
      ],
      total: 1,
      page: 1,
      pageSize: 5
    };

    service.getTickets(1, 5).subscribe(response => {
      expect(response.items.length).toBe(1);
      expect(response.total).toBe(1);
    });

    const req = httpMock.expectOne(request => 
      request.url === `${API_URL}/tickets` && 
      request.params.has('page') &&
      request.params.has('pageSize')
    );
    expect(req.request.method).toBe('GET');
    req.flush(mockTickets);
  });

  it('should fetch tickets with status filter', () => {
    const mockTickets = { items: [], total: 0, page: 1, pageSize: 5 };

    service.getTickets(1, 5, 1).subscribe();

    const req = httpMock.expectOne(request => 
      request.url === `${API_URL}/tickets` &&
      request.params.get('status') === '1'
    );
    req.flush(mockTickets);
  });

  it('should fetch tickets with priority filter (including 0 for Baja)', () => {
    const mockTickets = { items: [], total: 0, page: 1, pageSize: 5 };

    service.getTickets(1, 5, undefined, 0).subscribe();

    const req = httpMock.expectOne(request => 
      request.url === `${API_URL}/tickets` &&
      request.params.get('priority') === '0'
    );
    req.flush(mockTickets);
  });

  it('should create a new ticket', () => {
    const newTicket = { title: 'New Ticket', description: 'New Description', priority: 2 };
    const mockResponse = { id: 5, ...newTicket, status: 0, createdAt: new Date() };

    service.createTicket(newTicket as any).subscribe(response => {
      expect(response.id).toBe(5);
      expect(response.title).toBe('New Ticket');
    });

    const req = httpMock.expectOne(`${API_URL}/tickets`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newTicket);
    req.flush(mockResponse);
  });

  it('should update an existing ticket', () => {
    const ticketId = 1;
    const updateData = { title: 'Updated Ticket', status: 1, priority: 1 };
    const mockResponse = { id: ticketId, ...updateData };

    service.updateTicket(ticketId, updateData as any).subscribe(response => {
      expect(response.id).toBe(ticketId);
      expect(response.title).toBe('Updated Ticket');
    });

    const req = httpMock.expectOne(`${API_URL}/tickets/${ticketId}`);
    expect(req.request.method).toBe('PUT');
    req.flush(mockResponse);
  });

  it('should delete a ticket', () => {
    const ticketId = 1;

    service.deleteTicket(ticketId).subscribe();

    const req = httpMock.expectOne(`${API_URL}/tickets/${ticketId}`);
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  });
});
