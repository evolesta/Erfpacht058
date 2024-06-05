import { Component, OnInit, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { HttpHelperService } from '../../base/services/http-helper.service';
import { CommonModule, DatePipe } from '@angular/common';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-facturatie',
  standalone: true,
  imports: [MatTableModule, MatIconModule, MatButtonModule, DatePipe, MatSortModule, MatPaginatorModule, CommonModule, RouterModule],
  templateUrl: './facturatie.component.html',
  styleUrl: './facturatie.component.css'
})
export class FacturatieComponent implements OnInit {

  displayedColumns: string[] = ['id', 'factureringsPeriode', 'status', 'aanmaakDatum', 'invoices'];
  facturaties: MatTableDataSource<any>;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  perioden = {
    0: 'Juni', 1: 'December'
  };
  statussen = {
    0: 'Nieuw', 1: 'In behandeling', 2: 'Succesvol', 3: 'Mislukt'
  };

  constructor(private http: HttpHelperService) {}

  ngOnInit(): void {
      this.getFacturaties();
  }

  getFacturaties(): void {
    this.http.get('/factuurJob').subscribe(resp => {
      const response:any = resp.body;
      this.facturaties = new MatTableDataSource(response);

      this.facturaties.sort = this.sort;
      this.facturaties.paginator = this.paginator;
    });
  }

  // Download ZIP bestand met facturen
  downloadFacturen(id: string, filename: string): void {
    this.http.download("/factuurJob/download/" + id).subscribe(blob => {
      // Initieer download
      const link = document.createElement('a');
      link.href = window.URL.createObjectURL(blob);
      link.download = filename;
      link.click();
    });
  }
}
