import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddRapportageComponent } from './add-rapportage.component';

describe('AddRapportageComponent', () => {
  let component: AddRapportageComponent;
  let fixture: ComponentFixture<AddRapportageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddRapportageComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddRapportageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
