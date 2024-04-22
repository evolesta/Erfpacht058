import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SjablonenComponent } from './sjablonen.component';

describe('SjablonenComponent', () => {
  let component: SjablonenComponent;
  let fixture: ComponentFixture<SjablonenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SjablonenComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SjablonenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
