import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModSjablonenComponent } from './mod-sjablonen.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('ModSjablonenComponent', () => {
  let component: ModSjablonenComponent;
  let fixture: ComponentFixture<ModSjablonenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModSjablonenComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter ([]), provideAnimations()]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ModSjablonenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
