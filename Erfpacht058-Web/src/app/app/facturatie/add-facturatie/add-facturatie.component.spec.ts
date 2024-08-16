import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddFacturatieComponent } from './add-facturatie.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('AddFacturatieComponent', () => {
  let component: AddFacturatieComponent;
  let fixture: ComponentFixture<AddFacturatieComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddFacturatieComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter ([]), provideAnimations()]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddFacturatieComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
