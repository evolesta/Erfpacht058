import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { HttpHelperService } from '../../base/services/http-helper.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-bestand',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatIconModule, MatButtonModule, MatSelectModule],
  templateUrl: './bestand.component.html',
  styleUrl: './bestand.component.css'
})
export class BestandComponent implements OnInit {

  bestandForm = new FormGroup({
    naam: new FormControl(''),
    soortBestand: new FormControl(''),
    beschrijving: new FormControl(''),
  });

  bestandsId: string;

  constructor(private formbuilder: FormBuilder,
    private http: HttpHelperService,
    private router: Router,
    private route: ActivatedRoute) {}

  ngOnInit(): void {
      this.bestandsId = this.route.snapshot.paramMap.get('id');
      this.initBestand();
  }

  initBestand(): void {
    this.http.get('/bestand/' + this.bestandsId).subscribe(resp => {
      const response:any = resp.body;
      this.bestandForm.patchValue(response);
    });
  }

  submitBestand(): void {
    this.http.put('/bestand/' + this.bestandsId, this.bestandForm.value).subscribe(resp => {
      this.router.navigateByUrl('app');
    });
  }
}
