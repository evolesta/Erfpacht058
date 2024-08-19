import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModOvereenkomstComponent } from './mod-overeenkomst.component';
import { HttpClient, provideHttpClient } from '@angular/common/http';
import { HttpClientTestingModule, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('ModOvereenkomstComponent', () => {
  let component: ModOvereenkomstComponent;
  let fixture: ComponentFixture<ModOvereenkomstComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModOvereenkomstComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([]), provideAnimations()]
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
