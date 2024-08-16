import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModUserComponent } from './mod-user.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('ModUserComponent', () => {
  let component: ModUserComponent;
  let fixture: ComponentFixture<ModUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModUserComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([]), provideAnimations()]

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
