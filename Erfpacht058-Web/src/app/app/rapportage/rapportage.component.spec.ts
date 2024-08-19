import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RapportageComponent } from './rapportage.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('RapportageComponent', () => {
  let component: RapportageComponent;
  let fixture: ComponentFixture<RapportageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RapportageComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([]), provideAnimations()]

    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RapportageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
