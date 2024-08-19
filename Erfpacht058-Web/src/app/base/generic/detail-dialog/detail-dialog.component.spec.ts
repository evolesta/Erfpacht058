import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailDialogComponent } from './detail-dialog.component';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('DetailDialogComponent', () => {
  let component: DetailDialogComponent;
  let fixture: ComponentFixture<DetailDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DetailDialogComponent, MatDialogModule],
      providers: [
        { provide: MAT_DIALOG_DATA, useValue: {}},
        { provide: MatDialogRef, useValue: {} },
        provideHttpClient(), provideHttpClientTesting(), provideAnimations(),
      ]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DetailDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
