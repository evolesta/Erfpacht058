import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BestandComponent } from './bestand.component';

describe('BestandComponent', () => {
  let component: BestandComponent;
  let fixture: ComponentFixture<BestandComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BestandComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BestandComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
