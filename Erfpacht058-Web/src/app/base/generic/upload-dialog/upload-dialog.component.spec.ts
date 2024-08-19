import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadDialogComponent } from './upload-dialog.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';

describe('UploadDialogComponent', () => {
  let component: UploadDialogComponent;
  let fixture: ComponentFixture<UploadDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UploadDialogComponent, MatDialogModule],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter ([]), provideAnimations(),
      { provide: MAT_DIALOG_DATA, useValue: {}},
      { provide: MatDialogRef, useValue: {} }]

    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UploadDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
