import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BasetemplateComponent } from './basetemplate.component';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('BasetemplateComponent', () => {
  let component: BasetemplateComponent;
  let fixture: ComponentFixture<BasetemplateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BasetemplateComponent],
      providers: [provideRouter([]), provideAnimations()]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BasetemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
