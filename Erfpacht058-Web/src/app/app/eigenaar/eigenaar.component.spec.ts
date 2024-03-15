import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EigenaarComponent } from './eigenaar.component';

describe('EigenaarComponent', () => {
  let component: EigenaarComponent;
  let fixture: ComponentFixture<EigenaarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EigenaarComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EigenaarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
