import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { HttpHelperService } from '../../../base/services/http-helper.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSelectModule } from '@angular/material/select';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-mod-user',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, MatButtonModule, ReactiveFormsModule, MatIconModule, MatSelectModule, MatSlideToggleModule, CommonModule],
  templateUrl: './mod-user.component.html',
  styleUrl: './mod-user.component.css'
})
export class ModUserComponent implements OnInit {

  userForm = new FormGroup({
    naam: new FormControl('', Validators.required),
    voornamen: new FormControl('', Validators.required),
    emailadres: new FormControl('', Validators.required),
    wachtwoord: new FormControl('', Validators.required),
    wachtwoord2: new FormControl('', Validators.required),
    role: new FormControl('', Validators.required),
    actief: new FormControl('', Validators.required)
  });

  edit: boolean;
  gebruikerId: string;

  constructor(private http: HttpHelperService,
    private router: Router,
    private route: ActivatedRoute) {}

  ngOnInit(): void {
    if (this.router.url.includes('edit')) {
      this.edit = true;
      this.gebruikerId = this.route.snapshot.paramMap.get('id');

      this.initGebruiker();
    }
  }

  initGebruiker(): void {
    this.http.get('/gebruikers/' + this.gebruikerId).subscribe(resp => {
      const response:any = resp.body;
      this.userForm.patchValue(response);
    });
  }

  modGebruiker(): void {
    if (this.edit)
      this.editGebruiker();
    else 
      this.addGebruiker();
  }

  addGebruiker(): void {
    if (this.userForm.status === "VALID") {
      // Controleer of wachtwoorden matchen
      if (this.passwordMatch(this.userForm.controls.wachtwoord.value, this.userForm.controls.wachtwoord2.value)) {
        this.http.post('/gebruikers', this.userForm.value).subscribe(resp => {
          this.router.navigateByUrl('app/usermanagement');
        });
      }
    }
  }

  editGebruiker(): void {
    // Bypass wachtwoord Required Validator - Wachtwoord wordt bij backend genegeerd
    this.userForm.controls.wachtwoord.setValue('x');
    this.userForm.controls.wachtwoord2.setValue('x');

    if (this.userForm.status === "VALID") {
      this.http.put('/gebruikers/' + this.gebruikerId, this.userForm.value).subscribe(resp => {
        this.router.navigateByUrl('app/usermanagement');
      });
    }
  }

  // ++ Helper functies

  // Controleer of wachtwoord overeenkomen
  passwordMatch(password1, password2): boolean {
    return password1 === password2;
  }
}
