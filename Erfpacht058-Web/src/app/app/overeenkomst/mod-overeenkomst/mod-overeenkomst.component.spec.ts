import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModOvereenkomstComponent } from './mod-overeenkomst.component';

describe('ModOvereenkomstComponent', () => {
  let component: ModOvereenkomstComponent;
  let fixture: ComponentFixture<ModOvereenkomstComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModOvereenkomstComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ModOvereenkomstComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
