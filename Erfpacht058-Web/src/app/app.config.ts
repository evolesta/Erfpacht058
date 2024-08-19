import { ApplicationConfig, LOCALE_ID, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimations } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, HttpClient, provideHttpClient } from '@angular/common/http';
import { AuthenticateInterceptor } from './base/services/authenticate.interceptor';
import { CommonModule, NgIf } from '@angular/common';
import { provideClientHydration, provideProtractorTestingSupport } from '@angular/platform-browser';
import { provideHttpClientTesting } from '@angular/common/http/testing';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(),
    provideHttpClientTesting(),
    provideRouter(routes),
    provideProtractorTestingSupport(),
    provideClientHydration(),
    importProvidersFrom(CommonModule),
    provideAnimations(),
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticateInterceptor, multi: true },
  ]
};
