import { TestBed } from '@angular/core/testing';

import { HttpHelperService } from './http-helper.service';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('HttpHelperService', () => {
  let service: HttpHelperService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()]
    });
    service = TestBed.inject(HttpHelperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
