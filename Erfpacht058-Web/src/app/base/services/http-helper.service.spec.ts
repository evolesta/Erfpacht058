import { TestBed } from '@angular/core/testing';

import { HttpHelperService } from './http-helper.service';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { environment } from '../../../environments/environment';
import { provideHttpClient } from '@angular/common/http';

describe('HttpHelperService', () => {
  let service: HttpHelperService;
  let httpTestingController: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient() ,provideHttpClientTesting()]
    });
    service = TestBed.inject(HttpHelperService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('moet data kunnen opvragen via een HTTP-verzoek via GET', () => {
    // Arrange
    const mockData = {
      Name: "de Vries",
      FirstName: "Jan",
      Age: 30,
      Gender: "Man"
    };

    // Act
    service.get('/test/data').subscribe(data => {
      expect(data.body).toEqual(mockData);
    });

    // Assert
    const req = httpTestingController.expectOne(environment.apiURL + '/test/data');
    expect(req.request.method).toBe('GET');

    req.flush(mockData);
  });
});
