import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdresComponent } from './adres.component';
import { HttpClient, provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { routes } from '../../app.routes';
import { provideAnimations } from '@angular/platform-browser/animations';
describe('AdresComponent', () => {
  let component: AdresComponent;
  let fixture: ComponentFixture<AdresComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdresComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter ([]), provideAnimations()]

    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
