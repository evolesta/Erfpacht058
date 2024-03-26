import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { HttpHelperService } from '../../../base/services/http-helper.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-mod-overeenkomst',
  standalone: true,
  imports: [ReactiveFormsModule, MatInputModule, MatFormFieldModule, MatIconModule, MatButtonModule],
  templateUrl: './mod-overeenkomst.component.html',
  styleUrl: './mod-overeenkomst.component.css'
})
export class ModOvereenkomstComponent implements OnInit {

  addOvereenkomstForm = new FormGroup({
    dossiernummer: new FormControl(),
    ingangsdatum: new FormControl('', Validators.required),
    einddatum: new FormControl(),
    grondwaarde: new FormControl(),
    datumAkte: new FormControl(),
    rentepercentage: new FormControl()
  });

  eigendomId: string;

  constructor(private http: HttpHelperService,
    private route: ActivatedRoute,
    private router: Router) {}

    ngOnInit(): void {
        this.eigendomId = this.route.snapshot.paramMap.get('id');
    }

  addOvereenkomst(): void {
    this.http.post('/eigendom/overeenkomst/' + this.eigendomId, this.addOvereenkomstForm.value).subscribe(resp => {
      this.router.navigateByUrl('app');
    });
  }
}
