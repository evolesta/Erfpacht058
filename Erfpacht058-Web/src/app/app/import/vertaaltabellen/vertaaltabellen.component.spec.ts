import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VertaaltabellenComponent } from './vertaaltabellen.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('VertaaltabellenComponent', () => {
  let component: VertaaltabellenComponent;
  let fixture: ComponentFixture<VertaaltabellenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VertaaltabellenComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter ([]), provideAnimations()]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(VertaaltabellenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
