import { ComponentFixture, TestBed } from '@angular/core/testing';

import { KadasterComponent } from './kadaster.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('KadasterComponent', () => {
  let component: KadasterComponent;
  let fixture: ComponentFixture<KadasterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [KadasterComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([]), provideAnimations()]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(KadasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
