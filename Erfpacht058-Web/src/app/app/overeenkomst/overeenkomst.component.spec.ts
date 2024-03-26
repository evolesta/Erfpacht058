import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OvereenkomstComponent } from './overeenkomst.component';

describe('OvereenkomstComponent', () => {
  let component: OvereenkomstComponent;
  let fixture: ComponentFixture<OvereenkomstComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OvereenkomstComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(OvereenkomstComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
