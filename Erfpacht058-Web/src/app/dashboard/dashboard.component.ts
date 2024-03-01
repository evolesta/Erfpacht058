import { Component, OnInit } from '@angular/core';
import { HttpHelperService } from '../base/services/http-helper.service';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import { CommonModule, registerLocaleData } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import {MatGridListModule} from '@angular/material/grid-list';
import { SearchDialogComponent } from '../base/generic/search-dialog/search-dialog.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [MatButtonModule, MatIconModule, CommonModule, RouterModule, MatGridListModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {

  // Globale variabelen
  eigendom: any; // Bevat geselecteerde Eigendom object
  loaded: boolean;

  constructor(private http: HttpHelperService,
    private dialog: MatDialog) {}

  ngOnInit(): void {
    this.initEigendom();
  }

  // Functie die informatie over het eigendom verkrijgt indien geselecteerd in localStorage
  initEigendom(): void {
    const eigendomId = localStorage.getItem('eigendomId'); // Verkrijg eigendomId uit localstorage

    // Als eigendom geselecteerd is verkrijg informatie van back-end
    if (eigendomId != null) {
      this.http.get('/eigendom/' + eigendomId).subscribe(resp => {
        const response:any = resp.body;
        this.eigendom = response;
        this.loaded = true;
      });
    }
  }

  openZoekDialog(): void {
    const dialogRef = this.dialog.open(SearchDialogComponent, {
      data: {
        endpoint: '/eigendom'
      }
    });
  }
}
