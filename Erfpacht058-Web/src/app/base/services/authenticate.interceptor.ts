import { HttpEvent, HttpHandler, HttpInterceptor, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable, EMPTY, finalize } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { HelperService } from './helper.service';
import { LoadingSpinnerService } from '../generic/loading-spinner/loading-spinner.service';

@Injectable()
export class AuthenticateInterceptor implements HttpInterceptor {

  // Publieke routes die genegeerd worden
  private publicRoutes = [
    environment.apiURL + '/token'
  ];

  constructor(private router: Router,
    private helper: HelperService,
    private spinner: LoadingSpinnerService) {}

  // Interceptor die voor beveiligde routes automatisch de bearer token toevoegd aan de header van het request
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Alleen uitvoeren als het geen publieke route betreft
    this.spinner.show(); // toon laad spinner
    
    if (!this.publicRoutes.includes(req.url))
    {
      const token = localStorage.getItem('token');

      // Controleer of de token aanwezig is
      if (token == null) {
        // Token is niet aanwezig, navigeer de gebruiker naar login
        this.spinner.hide();
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

        // Geef gewijzigde request terug
        return next.handle(req).pipe(
          finalize(() => this.spinner.hide())
        );
      }
      else {
        // Token is verlopen - navigeer de gebruiker naar login
        this.spinner.hide();
        this.router.navigateByUrl('');
        return EMPTY;
      }
    } else {
      // publieke routes - geeft request direct terug
      return next.handle(req).pipe(
        finalize(() => this.spinner.hide())
      );
    }
  }
}
