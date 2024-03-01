import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { HttpHelperService } from '../../../base/services/http-helper.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-eigendom',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule],
  templateUrl: './add-eigendom.component.html',
  styleUrl: './add-eigendom.component.css'
})
export class AddEigendomComponent {

  addEigendomForm = new FormGroup({
    relatienummer: new FormControl(),
    ingangsdatum: new FormControl('', Validators.required),
    complexnummer: new FormControl(),
    economischeWaarde: new FormControl(),
    verzekerdeWaarde: new FormControl(),
    notities: new FormControl()
  });

  constructor(private http: HttpHelperService,
    private router: Router) {}

  addEigendom(): void {
    // Creeer nieuw eigendom in de api
    this.http.post('/eigendom', this.addEigendomForm.value).subscribe(resp => {
      const response:any = resp.body;

      localStorage.setItem('eigendomId', response.id); // Selecteer nieuwe eigendom in localStorage van browser
      this.router.navigateByUrl('/app'); // Ga terug naar dashboard via de router
    });
  }

}
