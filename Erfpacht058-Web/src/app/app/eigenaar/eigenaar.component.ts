import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormField, MatInputModule } from '@angular/material/input';
import { HttpHelperService } from '../../base/services/http-helper.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-eigenaar',
  standalone: true,
  imports: [ReactiveFormsModule, MatInputModule, MatFormField, MatButtonModule, MatIconModule],
  templateUrl: './eigenaar.component.html',
  styleUrl: './eigenaar.component.css'
})
export class EigenaarComponent implements OnInit {

  eigendomId: string;
  eigenaarId: string;
  edit: boolean;
  vanuitEigendom: boolean;

  eigenaarForm = new FormGroup({
    naam: new FormControl('', Validators.required),
    voorletters: new FormControl('', Validators.required),
    voornamen: new FormControl(),
    straatnaam: new FormControl('', Validators.required),
    huisnummer: new FormControl('', Validators.required),
    toevoeging: new FormControl(),
    postcode: new FormControl('', Validators.required),
    woonplaats: new FormControl('', Validators.required),
    debiteurnummer: new FormControl(),
    ingangsdatum: new FormControl('', Validators.required),
    einddatum: new FormControl()
  });

  constructor(private http: HttpHelperService,
    private route: ActivatedRoute,
    private router: Router) {}

  ngOnInit(): void {
      this.initEigenaar();
  }

  initEigenaar(): void {
    // Alleen uitvoeren wanneer bewerkmodus actief is
    if (this.router.url.includes('edit')) {
      this.edit = true;
      this.eigenaarId = this.route.snapshot.paramMap.get('id');

      this.http.get('/eigenaar/' + this.eigenaarId).subscribe(resp => {
        const response:any = resp.body;
        const pipe = new DatePipe("en-US");
        response.ingangsdatum = pipe.transform(response.ingangsdatum, 'yyyy-MM-dd');
        response.einddatum = pipe.transform(response.einddatumm, 'yyyy-MM-dd');
        this.eigenaarForm.patchValue(response);
      });
    }
    
    // Check of eigenaar vanuit eigendom gekoppeld wordt
    if (this.router.url.includes('add/0')) {
      this.vanuitEigendom = false;
    }
    else 
      this.vanuitEigendom = true;
      this.eigendomId = this.route.snapshot.paramMap.get('id');
  }

  modEigenaar(): void {
    if (this.edit)
      this.editEigenaar();
    else
      this.addEigenaar();
  }

  addEigenaar(): void {
    if (this.eigenaarForm.status == 'VALID') {
      if (this.vanuitEigendom) {
        // Voeg een eigenaar toe en leg meteen een relatie
        this.http.post('/eigendom/eigenaar/' + this.eigendomId, this.eigenaarForm.value).subscribe(resp => {
          this.router.navigateByUrl('app');
        });
      }
      else {
        // voeg enkel een eigenaar toe
        this.http.post('/eigenaar', this.eigenaarForm.value).subscribe(resp => {
          this.router.navigateByUrl('app/eigenaar');
        })
      }
    }
  }

  // functie voor het bewerken van een bestaande eigenaar
  editEigenaar(): void {
    if (this.eigenaarForm.status == 'VALID') {
      this.http.put('/eigenaar/' + this.eigenaarId, this.eigenaarForm.value).subscribe(resp => {
        this.router.navigateByUrl('app');
      });
    }
  }
}
