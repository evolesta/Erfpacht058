import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SjablonenComponent } from './sjablonen.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('SjablonenComponent', () => {
  let component: SjablonenComponent;
  let fixture: ComponentFixture<SjablonenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SjablonenComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([]), provideAnimations()]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SjablonenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
