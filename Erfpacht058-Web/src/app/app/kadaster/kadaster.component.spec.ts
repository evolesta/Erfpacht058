import { ComponentFixture, TestBed } from '@angular/core/testing';

import { KadasterComponent } from './kadaster.component';

describe('KadasterComponent', () => {
  let component: KadasterComponent;
  let fixture: ComponentFixture<KadasterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [KadasterComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(KadasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
