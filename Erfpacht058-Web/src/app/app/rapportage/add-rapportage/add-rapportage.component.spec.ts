import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddRapportageComponent } from './add-rapportage.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('AddRapportageComponent', () => {
  let component: AddRapportageComponent;
  let fixture: ComponentFixture<AddRapportageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddRapportageComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([]), provideAnimations()]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddRapportageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
