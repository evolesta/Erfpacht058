import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { HttpHelperService } from '../../base/services/http-helper.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe, formatDate } from '@angular/common';

@Component({
  selector: 'app-add-eigendom',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule],
  templateUrl: './eigendom.component.html',
  styleUrl: './eigendom.component.css'
})
export class EigendomComponent implements OnInit {

  eigendomId: string;
  edit: boolean;

  eigendomForm = new FormGroup({
    relatienummer: new FormControl(),
    ingangsdatum: new FormControl('', Validators.required),
    einddatum: new FormControl(),
    complexnummer: new FormControl(),
    economischeWaarde: new FormControl(),
    verzekerdeWaarde: new FormControl(),
    notities: new FormControl()
  });

  constructor(private http: HttpHelperService,
    private router: Router,
    private route: ActivatedRoute) {}

  ngOnInit(): void {
        this.initEigendom();
  }

  // Initieer eigendom van de api alleen als het om een bewerking gaat
  initEigendom(): void {
    if (this.router.url.includes('edit')) {
      this.edit = true; 

      this.eigendomId = this.route.snapshot.paramMap.get('id');
      this.http.get('/eigendom/' + this.eigendomId).subscribe(resp => {
        const response:any = resp.body;

        const pipe = new DatePipe("en-US");
        response.ingangsdatum = pipe.transform(response.ingangsdatum, 'yyyy-MM-dd'); // Converteer Timestamp uit db naar corr. datum format
        response.einddatum = pipe.transform(response.einddatum, 'yyyy-MM-dd'); // Converteer Timestamp uit db naar corr. datum format

        this.eigendomForm.patchValue(response);
      });
    }
  }

  // Voeg een nieuw eigendom toe of wijzig een bestaande
  modEigendom(): void {
    if (this.edit)
      this.editEigendom();
    else
      this.addEigendom();
  }

  addEigendom(): void {
    // Creeer nieuw eigendom in de api
    if (this.eigendomForm.status === 'VALID') {
      this.http.post('/eigendom', this.eigendomForm.value).subscribe(resp => {
        const response:any = resp.body;
  
        localStorage.setItem('eigendomId', response.id); // Selecteer nieuwe eigendom in localStorage van browser
        this.router.navigateByUrl('/app'); // Ga terug naar dashboard via de router
      });
    }
  }

  editEigendom(): void {
    // Creeer nieuw eigendom in de api
    if (this.eigendomForm.status === 'VALID') {
      this.http.put('/eigendom/' + this.eigendomId, this.eigendomForm.value).subscribe(resp => {
        this.router.navigateByUrl('/app'); // Ga terug naar dashboard via de router
      });
    }
  }

}
