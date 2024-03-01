import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEigendomComponent } from './add-eigendom.component';

describe('AddEigendomComponent', () => {
  let component: AddEigendomComponent;
  let fixture: ComponentFixture<AddEigendomComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddEigendomComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddEigendomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
