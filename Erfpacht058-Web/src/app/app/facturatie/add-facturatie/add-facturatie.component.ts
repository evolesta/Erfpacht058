import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpHelperService } from '../../../base/services/http-helper.service';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-facturatie',
  standalone: true,
  imports: [ReactiveFormsModule, MatInputModule, MatFormFieldModule, MatSelectModule, MatButtonModule, MatIconModule],
  templateUrl: './add-facturatie.component.html',
  styleUrl: './add-facturatie.component.css'
})
export class AddFacturatieComponent {

  addFacturatieForm = new FormGroup({
    factureringsPeriode: new FormControl("", Validators.required)
  });

  constructor(private http: HttpHelperService,
    private router: Router
  ) {}

  addFacturatieJob(): void {
    this.http.post('/factuurJob', this.addFacturatieForm.value).subscribe(resp => {
      this.router.navigateByUrl('app/facturatie');
    });
  }

}
