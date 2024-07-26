import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpHelperService } from '../../base/services/http-helper.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-kadaster',
  standalone: true,
  imports: [MatInputModule, MatFormFieldModule, MatButtonModule, MatInputModule, ReactiveFormsModule, MatIconModule],
  templateUrl: './kadaster.component.html',
  styleUrl: './kadaster.component.css'
})
export class KadasterComponent implements OnInit {

  kadasterForm = new FormGroup({
    bagid: new FormControl('', Validators.required),
    oppervlakte: new FormControl({value: '', disabled: true}),
    bouwjaar: new FormControl({value: '', disabled: true}),
    gebruiksdoel: new FormControl({value: '', disabled: true})
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
      this.initKadaster();
    }  
  }

  initKadaster(): void {  
    this.http.get('/eigendom/' + this.eigendomId).subscribe(resp => {
      const response:any = resp.body;
      this.kadasterForm.patchValue(response.kadaster);
    });
  }

  modKadaster(): void {
    if (this.edit) this.editKadaster();
    else this.addKadaster();
  }

  addKadaster(): void {
    this.http.post('/eigendom/kadaster/' + this.eigendomId, this.kadasterForm.value).subscribe(resp => {
      this.router.navigateByUrl('app');
    });
  }

  editKadaster(): void {
    this.http.put('/eigendom/kadaster/' + this.eigendomId, this.kadasterForm.value).subscribe(resp => {
      this.router.navigateByUrl('app');
    });
  }

  syncKadaster(): void {

  }
}
