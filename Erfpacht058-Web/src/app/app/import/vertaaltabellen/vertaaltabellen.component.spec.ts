import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VertaaltabellenComponent } from './vertaaltabellen.component';

describe('VertaaltabellenComponent', () => {
  let component: VertaaltabellenComponent;
  let fixture: ComponentFixture<VertaaltabellenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VertaaltabellenComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(VertaaltabellenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
