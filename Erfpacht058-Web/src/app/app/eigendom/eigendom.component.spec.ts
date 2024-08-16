import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EigendomComponent } from './eigendom.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('EigendomComponent', () => {
  let component: EigendomComponent;
  let fixture: ComponentFixture<EigendomComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EigendomComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([]), provideAnimations()]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EigendomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
