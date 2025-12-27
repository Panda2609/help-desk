export interface Ticket {
  id: number;
  title: string;
  description: string;
  priority: number;
  status: number;
  createdAt: string;
  updatedAt: string;
  resolvedAt?: string;
  createdByUserId: number;
  assignedToUserId?: number;
  notes?: string;
}

export interface CreateTicketRequest {
  title: string;
  description: string;
  priority: number;
  assignedToUserId?: number;
}

export interface UpdateTicketRequest {
  title?: string;
  description?: string;
  priority?: number;
  status?: number;
  assignedToUserId?: number;
  notes?: string;
}
