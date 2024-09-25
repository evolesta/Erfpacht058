import { Component, Inject } from '@angular/core';
import { Form, FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { HttpHelperService } from '../../services/http-helper.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-upload-dialog',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule, MatIconModule, MatFormFieldModule, MatInputModule, ReactiveFormsModule, MatSelectModule, CommonModule],
  templateUrl: './upload-dialog.component.html',
  styleUrl: './upload-dialog.component.css'
})
export class UploadDialogComponent {

  uploadFileForm = new FormGroup({
    Files: this.formbuilder.array([])
  });

  filesArray: string[] = [];
  error: string;

  constructor(private formbuilder: FormBuilder,
    private http: HttpHelperService,
    @Inject(MAT_DIALOG_DATA) public data: {id: string},
    private dialogRef: MatDialogRef<UploadDialogComponent>) {}

  // Functie die de formControl files bijwerkt wanneer er bestanden zijn toegevoegd aan het formulier
  onFileChange(event: any): void {
    this.filesArray = [];
    this.uploadFileForm.controls.Files.clear();

    const files = event.target.files;
    const fileArray = this.uploadFileForm.get('Files') as FormArray;

    for (let i = 0; i < files.length; i++) {
      fileArray.push(this.formbuilder.control(files[i]));
      this.filesArray.push(files[i].name);
    }
  }

  // Upload de bestanden naar de back-end
  uploadFiles(): void {
    if (this.uploadFileForm.controls.Files.length > 0) {
      // Omdat er ook bestanden geupload worden, wordt de FormGroup omgezet naar FormData
      const formData = new FormData();
      const files = (this.uploadFileForm.get('Files') as FormArray).controls;

      for (let i = 0; i < files.length; i++) {
        formData.append('Files', (files[i] as FormControl).value);
      }

      // POST request met file upload
      this.http.post('/eigendom/bestand/' + this.data.id, formData).subscribe(resp => {
        this.dialogRef.close();
      });
    }
    else this.error = "Geen bestanden geselecteerd.";
  } 
}
 