import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAdresComponent } from './add-adres.component';

describe('AddAdresComponent', () => {
  let component: AddAdresComponent;
  let fixture: ComponentFixture<AddAdresComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddAdresComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddAdresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
