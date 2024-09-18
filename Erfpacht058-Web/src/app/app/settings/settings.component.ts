import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { HttpHelperService } from '../../base/services/http-helper.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.css',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, CommonModule],
})
export class SettingsComponent implements OnInit {

  settingsForm = new FormGroup({
    bagapi: new FormControl()
  });

  hide: boolean = true;

  constructor(private http: HttpHelperService) {}

  ngOnInit(): void {
    this.getSettings();
  }

  getSettings(): void {
    this.http.get('/settings').subscribe(resp => {
      this.settingsForm.patchValue(resp.body);
    });
  }

  saveSettings(): void {
    this.http.put('/settings', this.settingsForm.value).subscribe(resp => {
      this.getSettings();
    });
  }

}
