import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { HttpHelperService } from '../../../base/services/http-helper.service';
import { MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-rapportage',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, MatSelectModule],
  templateUrl: './add-rapportage.component.html',
  styleUrl: './add-rapportage.component.css'
})
export class AddRapportageComponent implements OnInit {

  addRapportageForm = new FormGroup({
    formaat: new FormControl('', Validators.required),
    templateId: new FormControl('', Validators.required),
    start: new FormControl(true)
  });

  templates: any;

  constructor(private http: HttpHelperService,
    private router: Router
  ) {}

  ngOnInit(): void {
      this.getTemplates();
  }

  getTemplates(): void {
    this.http.get('/template').subscribe(resp => {
      const response:any = resp.body;
      this.templates = response;
    });
  }

  newExport(): void {
    this.http.post('/export', this.addRapportageForm.value).subscribe(resp => {
      this.router.navigateByUrl('app/rapportage');
    });
  }
}
