import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HerzieningComponent } from './herziening.component';

describe('HerzieningComponent', () => {
  let component: HerzieningComponent;
  let fixture: ComponentFixture<HerzieningComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HerzieningComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HerzieningComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
