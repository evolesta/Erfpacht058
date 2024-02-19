import { Component, OnInit } from '@angular/core';
import { HttpHelperService } from '../../services/http-helper.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {

  constructor(private http: HttpHelperService) {}

  ngOnInit(): void {
  }
}
