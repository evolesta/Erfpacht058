import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { HttpHelperService } from '../../../base/services/http-helper.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-import',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, ReactiveFormsModule, MatSelectModule, CommonModule],
  templateUrl: './add-import.component.html',
  styleUrl: './add-import.component.css'
})
export class AddImportComponent implements OnInit {

  addImportForm = new FormGroup({
     csvFile: new FormControl('', [Validators.required, validateCsv]),
     translateModelId: new FormControl('', Validators.required)
  });

  translateModels: any;
  file: File | null = null;

  constructor(private http: HttpHelperService,
    private router: Router
  ) {}

  ngOnInit(): void {
      this.getTranslateModels();
  }

  getTranslateModels(): void {
    this.http.get('/translatemodel').subscribe(resp => {
      const response:any = resp.body;
      this.translateModels = response;
    });
  }

  onFileChange(event: any) {
    const file: File = event.target.files[0];

    if (file) {
      this.file = file;
    }
  }

  addImport(): void {
    const translateModelId = this.addImportForm.controls.translateModelId.value;
    const formData = new FormData();

    formData.append('csvFile', this.file, this.file.name);

    this.http.post('/import/' + translateModelId, formData).subscribe(resp => {
      this.router.navigateByUrl('app/import');
    });
  }
}

// Validator die checkt of de gebruiker een CSV bestand heeft geupload
export const validateCsv: ValidatorFn = (control: AbstractControl): { [key: string]: any } | null => {
  // verkrijg pad
  const file = control.value as string;
  
  // Alleen checken als er een waarde aanwezig is
  if (file) {
    const extension = file.split('.').pop().toLowerCase(); // verkrijg extensie
    const isCsv = extension === 'csv';
    return isCsv ? null : {'notCsv' : true};
  }

  return null;
};