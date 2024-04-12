import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModUserComponent } from './mod-user.component';

describe('ModUserComponent', () => {
  let component: ModUserComponent;
  let fixture: ComponentFixture<ModUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModUserComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ModUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
