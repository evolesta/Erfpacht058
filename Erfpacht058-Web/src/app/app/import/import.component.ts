import { Component, OnInit, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router, RouterModule } from '@angular/router';
import { HttpHelperService } from '../../base/services/http-helper.service';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { CommonModule, DatePipe } from '@angular/common';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { DetailDialogComponent } from '../../base/generic/detail-dialog/detail-dialog.component';

@Component({
  selector: 'app-import',
  standalone: true,
  imports: [MatButtonModule, MatIconModule, RouterModule, MatTableModule, MatPaginatorModule, DatePipe, MatSortModule, CommonModule],
  templateUrl: './import.component.html',
  styleUrl: './import.component.css'
})
export class ImportComponent implements OnInit {

  importsTable: MatTableDataSource<any>;
  displayedColumns: string[] = ['id', 'naam', 'model', 'status', 'aanmaakDatum', 'failedInfo'];
  statussen: string[] = ['Nieuw', 'In behandeling', 'Voltooid', 'Mislukt', 'Verwijderd'];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private http: HttpHelperService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
      this.getImports();
  }

  getImports(): void {
    this.http.get('/import').subscribe(resp => {
      const response:any = resp.body;
      this.importsTable = new MatTableDataSource(response);

      // koppel sort en paginator objecten
      this.importsTable.sort = this.sort;
      this.importsTable.paginator = this.paginator;
    });
  }

  showErrorDialog(message): void {
    this.dialog.open(DetailDialogComponent, {
      data: {
        message: 'Foutmelding: ' + message,
        title: 'Foutmelding'
      }
    });
  }
}
