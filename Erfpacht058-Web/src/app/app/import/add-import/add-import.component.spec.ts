import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddImportComponent } from './add-import.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('AddImportComponent', () => {
  let component: AddImportComponent;
  let fixture: ComponentFixture<AddImportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddImportComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([]), provideAnimations()]

    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddImportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
