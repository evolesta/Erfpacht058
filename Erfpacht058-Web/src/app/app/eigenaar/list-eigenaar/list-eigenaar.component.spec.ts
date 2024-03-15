import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListEigenaarComponent } from './list-eigenaar.component';

describe('ListEigenaarComponent', () => {
  let component: ListEigenaarComponent;
  let fixture: ComponentFixture<ListEigenaarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListEigenaarComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ListEigenaarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
