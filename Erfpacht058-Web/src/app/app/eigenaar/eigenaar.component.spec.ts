import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EigenaarComponent } from './eigenaar.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('EigenaarComponent', () => {
  let component: EigenaarComponent;
  let fixture: ComponentFixture<EigenaarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EigenaarComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([]), provideAnimations()]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EigenaarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
