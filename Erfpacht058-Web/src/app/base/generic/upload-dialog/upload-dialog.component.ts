import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-upload-dialog',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule, MatIconModule, MatFormFieldModule, MatInputModule, ReactiveFormsModule, MatSelectModule],
  templateUrl: './upload-dialog.component.html',
  styleUrl: './upload-dialog.component.css'
})
export class UploadDialogComponent {

  uploadFileForm = new FormGroup({
    naam: new FormControl('', Validators.required),
    soortBestand: new FormControl('', Validators.required),
    beschrijving: new FormControl(),
  });
  files: FormGroup = this.formBuilder.group({
    file: ['']
  });

  constructor(private formBuilder: FormBuilder) {}

  addFiles(event) {
    const files = (event.target as HTMLInputElement).files;
    this.files.patchValue({file: files});
  }
}
 