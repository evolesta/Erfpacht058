import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpHelperService } from '../../base/services/http-helper.service';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-adres',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule],
  templateUrl: './adres.component.html',
  styleUrl: './adres.component.css'
})
export class AdresComponent implements OnInit {

  eigendomId: string;
  edit: boolean;

  adresForm = new FormGroup({
    straatnaam: new FormControl('', Validators.required),
    huisnummer: new FormControl('', Validators.required),
    toevoeging: new FormControl(),
    huisletter: new FormControl(),
    postcode: new FormControl('', Validators.required),
    woonplaats: new FormControl('', Validators.required)
  }); 

  constructor(private route: ActivatedRoute,
    private http: HttpHelperService,
    private router: Router) {}

  ngOnInit(): void {
    this.eigendomId = this.route.snapshot.paramMap.get('id');
    
    // Controleer of edit of add modus actief moet worden
      if (this.router.url.includes('edit')) {
        this.edit = true;
        this.initAdres();
      }
  }

  initAdres(): void {
    // verkrijg eigendom object en pas toe op de formgroup
    this.http.get('/eigendom/' + this.eigendomId).subscribe(resp => {
      const response:any = resp.body;
      this.adresForm.patchValue(response.adres);
    });
  }

  modAdres(): void {
    if(this.edit) 
      this.editAdres();
    else
      this.addAdres();
  }

  addAdres(): void {
    this.http.post('/eigendom/adres/' + this.eigendomId, this.adresForm.value).subscribe(resp => {
      this.router.navigateByUrl('app');
    });
  }

  editAdres(): void {
    this.http.put('/eigendom/adres/' + this.eigendomId, this.adresForm.value).subscribe(resp => {
      this.router.navigateByUrl('app');
    });
  }
}
