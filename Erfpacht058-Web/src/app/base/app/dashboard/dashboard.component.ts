import { Component, OnInit } from '@angular/core';
import { HttpHelperService } from '../../services/http-helper.service';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [MatButtonModule, MatIconModule, CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {

  eigendomId: number;

  constructor(private http: HttpHelperService) {}

  ngOnInit(): void {
  }
}
