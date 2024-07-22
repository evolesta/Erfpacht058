import { Component, OnInit } from '@angular/core';
import {MatCardModule} from '@angular/material/card';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatButtonModule} from '@angular/material/button';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpHelperService } from '../services/http-helper.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HelperService } from '../services/helper.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [MatCardModule, MatFormFieldModule, MatInputModule, MatButtonModule, ReactiveFormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  error: boolean;

  loginForm = new FormGroup({
    emailadres: new FormControl('', Validators.required),
    wachtwoord: new FormControl('', Validators.required)
  });

  constructor(private http: HttpHelperService,
    private router: Router,
    private helper: HelperService) {}

  ngOnInit(): void {
      if (this.helper.tokenValidator()) {
        this.router.navigateByUrl('/app');
      }
  }

  login(): void {
    if (this.loginForm.status == 'VALID') {
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
}
