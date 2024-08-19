import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HerzieningComponent } from './herziening.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('HerzieningComponent', () => {
  let component: HerzieningComponent;
  let fixture: ComponentFixture<HerzieningComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HerzieningComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter ([]), provideAnimations()]

    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HerzieningComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
