import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { HttpHelperService } from '../../base/services/http-helper.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-herziening',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule],
  templateUrl: './herziening.component.html',
  styleUrl: './herziening.component.css'
})
export class HerzieningComponent implements OnInit {

  herzieningForm = new FormGroup({
    herzieningsdatum: new FormControl(),
    volgendeHerziening: new FormControl('', Validators.required)
  });

  edit: boolean;
  eigendomId: string;

  constructor(private http: HttpHelperService,
    private router: Router,
    private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.eigendomId = this.route.snapshot.paramMap.get('id');

      if (this.router.url.includes('/edit')) {
        this.edit = true;
        this.getHerziening();
      }
  }

  modHerziening(): void {
    if (this.edit)
      this.editHerziening();
    else
      this.addHerziening();
  }

  getHerziening(): void {
    this.http.get('/eigendom/' + this.eigendomId).subscribe(resp => {
      const response:any = resp.body;
      const datepipe = new DatePipe('en-US');
      response.herziening.herzieningsdatum = datepipe.transform(response.herziening.herzieningsdatum, 'yyyy-MM-dd');
      this.herzieningForm.patchValue(response.herziening);
    });
  }

  addHerziening(): void {
    this.http.post('/eigendom/herziening/' + this.eigendomId, this.herzieningForm.value).subscribe(resp => {
      this.router.navigateByUrl('app');
    });
  }

  editHerziening(): void {
    this.http.put('/eigendom/herziening/' + this.eigendomId, this.herzieningForm.value).subscribe(resp => {
      this.router.navigateByUrl('app');
    });
  }

  // Helper functie om volgende herzieningsdatum te berekenen
  berekenHerzienDatum(event: any): void {
    // Date object maken op huid. tijd en datum
    const currentDate = new Date();
    const aantalJaren: number = +event.target.value;

    // Voeg het aantal jaren erbij toe
    currentDate.setFullYear(currentDate.getFullYear() + aantalJaren);

    // Geef aangepaste datum terug
    const datepipe = new DatePipe('en-US');
    const newDate = datepipe.transform(currentDate, 'yyyy-MM-dd');
    this.herzieningForm.controls.herzieningsdatum.setValue(newDate);
  }
}
