import { HttpEvent, HttpHandler, HttpInterceptor, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable, EMPTY } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { HelperService } from './helper.service';

@Injectable()
export class AuthenticateInterceptor implements HttpInterceptor {

  // Publieke routes die genegeerd worden
  private publicRoutes = [
    environment.apiURL + '/token'
  ];

  constructor(private router: Router,
    private helper: HelperService) {}

  // Interceptor die voor beveiligde routes automatisch de bearer token toevoegd aan de header van het request
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Alleen uitvoeren als het geen publieke route betreft
    if (!this.publicRoutes.includes(req.url))
    {
      const token = localStorage.getItem('token');

      // Controleer of de token aanwezig is
      if (token == null) {
        // Token is niet aanwezig, navigeer de gebruiker naar login
        this.router.navigateByUrl('');
        return EMPTY;
      }

      // Controleer of de token nog geldig is
      if (this.helper.tokenValidator(token))
      {
        // Token is nog geldig - headers aanpassen en token toevoegen
        req = req.clone({
          headers: req.headers.set(
            'Authorization', 'Bearer ' + token
          )
        });
      }
      else {
        // Token is verlopen - navigeer de gebruiker naar login
        this.router.navigateByUrl('');
        return EMPTY;
      }
    }

    // Forceer het (gewijzigde) HTTP request naar de endpoint
    return next.handle(req);
  }
}
