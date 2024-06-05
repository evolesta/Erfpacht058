import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModVertaaltabelComponent } from './mod-vertaaltabel.component';

describe('ModVertaaltabelComponent', () => {
  let component: ModVertaaltabelComponent;
  let fixture: ComponentFixture<ModVertaaltabelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModVertaaltabelComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ModVertaaltabelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
