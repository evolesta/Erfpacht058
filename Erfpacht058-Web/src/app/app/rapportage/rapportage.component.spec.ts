import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RapportageComponent } from './rapportage.component';

describe('RapportageComponent', () => {
  let component: RapportageComponent;
  let fixture: ComponentFixture<RapportageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RapportageComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RapportageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
