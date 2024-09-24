import { TestBed } from '@angular/core/testing';
import { HTTP_INTERCEPTORS, HttpClient, HttpInterceptorFn, provideHttpClient, withInterceptors } from '@angular/common/http';

import { authInterceptor } from './auth.interceptor';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { HttpHelperService } from './http-helper.service';
import { environment } from '../../../environments/environment';
import { LoadingSpinnerService } from '../generic/loading-spinner/loading-spinner.service';
import { HelperService } from './helper.service';
import { Router } from '@angular/router';

describe('authInterceptor', () => {
  
  let httpMock: HttpTestingController;
  let httpService: HttpHelperService;
  let httpClient: HttpClient;
  let helperService: HelperService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        HttpHelperService,
        LoadingSpinnerService,
        {
          provide: HelperService,
          useValue: {
            tokenValidator: jasmine.createSpy('tokenValidator').and.returnValue(true)
          }
        },
        {
          provide: Router,
          useValue: {
            navigateByUrl: jasmine.createSpy('navigateByUrl')
          }
        },
        provideHttpClient(withInterceptors([authInterceptor])),
        provideHttpClientTesting(),
        {
          provide: HTTP_INTERCEPTORS,
          useValue: authInterceptor,
          multi: true
        }
      ]
    });
    
    httpMock = TestBed.inject(HttpTestingController);
    httpService = TestBed.inject(HttpHelperService);
    httpClient = TestBed.inject(HttpClient);
    helperService = TestBed.inject(HelperService);

    // Mock localStorage token
    spyOn(localStorage, 'getItem').and.callFake((key: string) => {
      // mock a token
      return key === 'token' ? 'mock-token' : null;
    });
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('moet een Authorization header toevoegen bij een valide jwt token', () => {

    	// Act
      httpService.get('/test').subscribe(resp => {
        // voer het te onderscheppen HTTP-verzoek uit
        expect(resp).toBeTruthy();
      });

      // Assert - Verwacht een http-verzoek
      const httpRequest = httpMock.expectOne(environment.apiURL + '/test');

      // Test of er een Authorization header is toegevoegd
      expect(httpRequest.request.headers.has('Authorization')).toBeTruthy(); // Check of er een Authorization header aanwezig is
      expect(httpRequest.request.headers.get('Authorization')).toBe('Bearer mock-token'); // Check of onze token aanwezig is

      httpRequest.flush({data: 'test'});
  });

  it('moet herleiden naar login als er geen token aanwezig is', () => {
    
    // Arrange
    (localStorage.getItem as jasmine.Spy).and.callFake((key: string) => null)

    // Act
    httpService.get('/test').subscribe({
      // Assert
      next: (resp) => fail('zou een EMPTY response moeten geven'),
      error: (err) => expect(err).toBeTruthy()
    });

    expect(TestBed.inject(Router).navigateByUrl).toHaveBeenCalledWith(''); // Valideer of de router herleiding is aangeroepen
  });

  it('moet herleiden naar login als de token verlopen is', () => {
    (helperService.tokenValidator as jasmine.Spy).and.returnValue(false); // zorg dat TokenValidator false teruggeeft

    httpService.get('/test').subscribe({
      next: (resp) => fail('zou een EMPTY response moeten geven'),
      error: (err) => expect(err).toBeTruthy()
    });
    
    expect(TestBed.inject(Router).navigateByUrl).toHaveBeenCalledWith(''); // valideer of de herleiding heeft plaatsgevonden
  });
});
