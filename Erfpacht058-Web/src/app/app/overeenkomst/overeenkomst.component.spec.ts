import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OvereenkomstComponent } from './overeenkomst.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('OvereenkomstComponent', () => {
  let component: OvereenkomstComponent;
  let fixture: ComponentFixture<OvereenkomstComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OvereenkomstComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([]), provideAnimations()]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(OvereenkomstComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
