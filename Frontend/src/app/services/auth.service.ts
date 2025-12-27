import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap, catchError, throwError } from 'rxjs';
import { LoginRequest, LoginResponse, User } from '../models/auth.model';
import { PLATFORM_ID, Inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5264/api/auth';
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    // Inicializar el usuario desde localStorage cuando el servicio se crea
    this.initializeAuthState();
  }

  private initializeAuthState(): void {
    if (!isPlatformBrowser(this.platformId)) {
      return;
    }
    const user = this.getUserFromStorage();
    if (user) {
      this.currentUserSubject.next(user);
    }
  }

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, credentials).pipe(
      tap(response => {
        if (isPlatformBrowser(this.platformId)) {
          try {
            localStorage.setItem('token', response.token);
            localStorage.setItem('user', JSON.stringify(response.user));
          } catch (e) {
            console.error('AuthService.login() - Error saving to localStorage:', e);
          }
        }
        this.currentUserSubject.next(response.user);
      }),
      catchError(error => {
        // Propagar el error al componente sin registrarlo como "unhandled"
        return throwError(() => error);
      })
    );
  }

  logout(): void {
    if (isPlatformBrowser(this.platformId)) {
      try {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
      } catch (e) {
        console.error('AuthService.logout() - Error clearing localStorage:', e);
      }
    }
    this.currentUserSubject.next(null);
  }

  getToken(): string | null {
    if (!isPlatformBrowser(this.platformId)) {
      return null;
    }
    try {
      const token = localStorage.getItem('token');
      return token;
    } catch (e) {
      console.error('AuthService.getToken() - Error accessing localStorage:', e);
      return null;
    }
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    const isAuth = !!token;
    return isAuth;
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  private getUserFromStorage(): User | null {
    if (!isPlatformBrowser(this.platformId)) {
      return null;
    }
    try {
      const user = localStorage.getItem('user');
      return user ? JSON.parse(user) : null;
    } catch (e) {
      console.error('AuthService.getUserFromStorage() - Error reading from localStorage:', e);
      return null;
    }
  }
}
