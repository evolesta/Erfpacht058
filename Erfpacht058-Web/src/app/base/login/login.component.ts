import { Component } from '@angular/core';
import {MatCardModule} from '@angular/material/card';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatButtonModule} from '@angular/material/button';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpHelperService } from '../services/http-helper.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, ReactiveFormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  error: boolean;

  loginForm = new FormGroup({
    emailadres: new FormControl('', Validators.required),
    wachtwoord: new FormControl('', Validators.required)
  });

  constructor(private http: HttpHelperService,
    private router: Router) {}

  login(): void {
    this.http.post('/token', this.loginForm.value).subscribe({
      next: (resp) => {
        const body:any = resp.body;
        localStorage.setItem('token', body.token);
        this.router.navigateByUrl('/app');
      },
      error: (err) => {
        this.error = true;
      }
    });
  }
}
