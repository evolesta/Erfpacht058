import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FacturatieComponent } from './facturatie.component';

describe('FacturatieComponent', () => {
  let component: FacturatieComponent;
  let fixture: ComponentFixture<FacturatieComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FacturatieComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FacturatieComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
