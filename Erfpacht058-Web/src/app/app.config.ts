import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { HTTP_INTERCEPTORS, HttpClientModule, provideHttpClient } from '@angular/common/http';
import { AuthenticateInterceptor } from './base/services/authenticate.interceptor';
import { CommonModule, NgIf } from '@angular/common';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimationsAsync(),
    importProvidersFrom(HttpClientModule, CommonModule),
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticateInterceptor, multi: true }
  ]
};
