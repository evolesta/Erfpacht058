import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModVertaaltabelComponent } from './mod-vertaaltabel.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('ModVertaaltabelComponent', () => {
  let component: ModVertaaltabelComponent;
  let fixture: ComponentFixture<ModVertaaltabelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModVertaaltabelComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([]), provideAnimations()]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ModVertaaltabelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
