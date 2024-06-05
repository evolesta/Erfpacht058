import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddFacturatieComponent } from './add-facturatie.component';

describe('AddFacturatieComponent', () => {
  let component: AddFacturatieComponent;
  let fixture: ComponentFixture<AddFacturatieComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddFacturatieComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddFacturatieComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
