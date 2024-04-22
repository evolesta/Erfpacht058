import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModSjablonenComponent } from './mod-sjablonen.component';

describe('ModSjablonenComponent', () => {
  let component: ModSjablonenComponent;
  let fixture: ComponentFixture<ModSjablonenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModSjablonenComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ModSjablonenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
