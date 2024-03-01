import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BasetemplateComponent } from './basetemplate.component';

describe('BasetemplateComponent', () => {
  let component: BasetemplateComponent;
  let fixture: ComponentFixture<BasetemplateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BasetemplateComponent]
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
