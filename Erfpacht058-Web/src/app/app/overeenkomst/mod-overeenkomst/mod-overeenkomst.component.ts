import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { HttpHelperService } from '../../../base/services/http-helper.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-mod-overeenkomst',
  standalone: true,
  imports: [ReactiveFormsModule, MatInputModule, MatFormFieldModule, MatIconModule, MatButtonModule, MatSelectModule],
  templateUrl: './mod-overeenkomst.component.html',
  styleUrl: './mod-overeenkomst.component.css'
})
export class ModOvereenkomstComponent implements OnInit {

  overeenkomstForm = new FormGroup({
    dossiernummer: new FormControl(),
    ingangsdatum: new FormControl('', Validators.required),
    einddatum: new FormControl(),
    grondwaarde: new FormControl(),
    datumAkte: new FormControl(),
    rentepercentage: new FormControl(),
    financien: new FormGroup({
      bedrag: new FormControl(),
      factureringsWijze: new FormControl(),
      frequentie: new FormControl(),
      factureringsPeriode: new FormControl('', Validators.required)
    }),
  });

  id: string;
  edit: boolean;

  constructor(private http: HttpHelperService,
    private route: ActivatedRoute,
    private router: Router) {}

    ngOnInit(): void {
        this.id = this.route.snapshot.paramMap.get('id');
        
        if (this.router.url.includes('edit')) {
          this.edit = true;
          this.initOvereenkomst();
        }
    }

    initOvereenkomst(): void {
      this.http.get('/overeenkomst/' + this.id).subscribe(resp => {
        const response:any = resp.body;
        const datepipe = new DatePipe('en-US');

        this.overeenkomstForm.patchValue(response);
        this.overeenkomstForm.controls.ingangsdatum.setValue(datepipe.transform(response.ingangsdatum, 'yyyy-MM-dd'));
        this.overeenkomstForm.controls.einddatum.setValue(datepipe.transform(response.einddatum, 'yyyy-MM-dd'));
        this.overeenkomstForm.controls.datumAkte.setValue(datepipe.transform(response.datumAkte, 'yyyy-MM-dd'));
      });
    }

    modOvereenkomst(): void {
      if (this.edit) this.editOvereenkomst();
      else this.addOvereenkomst();
    }

  addOvereenkomst(): void {
    this.http.post('/eigendom/overeenkomst/' + this.id, this.overeenkomstForm.value).subscribe(resp => {
      this.router.navigateByUrl('app');
    });
  }

  editOvereenkomst(): void {
    this.http.put('/overeenkomst/' + this.id, this.overeenkomstForm.value).subscribe(resp => {
      this.router.navigateByUrl('app');
    });
  }
}
