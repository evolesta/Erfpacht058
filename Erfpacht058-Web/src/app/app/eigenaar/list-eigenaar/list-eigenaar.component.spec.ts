import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListEigenaarComponent } from './list-eigenaar.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('ListEigenaarComponent', () => {
  let component: ListEigenaarComponent;
  let fixture: ComponentFixture<ListEigenaarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListEigenaarComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([]), provideAnimations()]
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
