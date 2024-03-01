import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EigendomComponent } from './eigendom.component';

describe('EigendomComponent', () => {
  let component: EigendomComponent;
  let fixture: ComponentFixture<EigendomComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EigendomComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EigendomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
