import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BestandComponent } from './bestand.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('BestandComponent', () => {
  let component: BestandComponent;
  let fixture: ComponentFixture<BestandComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BestandComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([]), provideAnimations()]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BestandComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
