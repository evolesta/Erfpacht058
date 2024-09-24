import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { inject } from '@angular/core';
import { LoadingSpinnerService } from '../generic/loading-spinner/loading-spinner.service';
import { HelperService } from './helper.service';
import { Router } from '@angular/router';
import { EMPTY, finalize } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // Publieke routes waarvoor geen authenticatie is vereist
  const publicRoutes = [
    environment.apiURL + '/token'
  ];

  // injecteer benodigde services
  const spinner = inject(LoadingSpinnerService);
  const helper = inject(HelperService);
  const router = inject(Router);

  spinner.show(); // laad spinner zien

  // Alleen uitvoeren als het geen publieke route betreft
  if (!publicRoutes.includes(req.url))
  {
    // Geen publieke route
    const token = localStorage.getItem('token'); // verkrijg token uit localStorage

    // Check of token aanwezig is
    if (token == null) {
      // geen token aanwezig - leid naar login
      spinner.hide();
      router.navigateByUrl('');
      return EMPTY;
    }
    // Controleer of token nog geldig is
    if (helper.tokenValidator())
    {
      // Token is nog geldig - voeg Authorize header toe aan Request
      req = req.clone({
        headers: req.headers.set(
          'Authorization', 'Bearer ' + token
        )
      });

      // Geef Request terug aan HttpClient
      return next(req).pipe(
        finalize(() => spinner.hide())
      );
    }
    else {
      // Token is verlopen
      spinner.hide();
      router.navigateByUrl('');
      return EMPTY;
    }
  }
  else {
    // publieke route - geef request terug
    return next(req).pipe(
      finalize(() => spinner.hide())
    );
  }
};
