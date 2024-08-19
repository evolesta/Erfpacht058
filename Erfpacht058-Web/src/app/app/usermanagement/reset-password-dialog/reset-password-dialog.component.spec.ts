import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetPasswordDialogComponent } from './reset-password-dialog.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

describe('ResetPasswordDialogComponent', () => {
  let component: ResetPasswordDialogComponent;
  let fixture: ComponentFixture<ResetPasswordDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ResetPasswordDialogComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([]), provideAnimations(),
      { provide: MAT_DIALOG_DATA, useValue: {}},
      { provide: MatDialogRef, useValue: {} },]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ResetPasswordDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
