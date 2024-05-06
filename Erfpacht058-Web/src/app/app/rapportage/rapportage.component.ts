import { CommonModule, DatePipe } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { HelperService } from '../../base/services/helper.service';
import { HttpHelperService } from '../../base/services/http-helper.service';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, MatSortModule, MatSortable } from '@angular/material/sort';
import { DetailDialogComponent } from '../../base/generic/detail-dialog/detail-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-rapportage',
  standalone: true,
  imports: [MatButtonModule, MatIconModule, RouterModule, CommonModule, MatTableModule, DatePipe, MatPaginator, MatSortModule],
  templateUrl: './rapportage.component.html',
  styleUrl: './rapportage.component.css'
})
export class RapportageComponent implements OnInit {

  rapportagesTable: MatTableDataSource<any>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  columns: string[] = ['id', 'naam', 'formaat', 'status', 'aanmaakDatum', 'download', 'opties'];
  statussen: string[] = ['Nieuw', 'In behandeling', 'Voltooid', 'Mislukt', 'Verwijderd'];
  formaten: string[] = ['PDF', 'Excel', 'CSV'];

  constructor(private http: HttpHelperService,
    public helper: HelperService,
    private dialog: MatDialog) {}

  ngOnInit(): void {
      this.getRapportages();
  }

  // Verkrijg rapportages en initieer tabel
  getRapportages(): void {
    this.http.get('/export').subscribe(resp => {
      const response:any = resp.body;
      this.rapportagesTable = new MatTableDataSource(response);

      this.rapportagesTable.paginator = this.paginator;
      this.rapportagesTable.sort = this.sort;
    });
  }

  // Download Export bestand
  downloadExport(id: string, filename: string): void {
    this.http.download("/export/download/" + id).subscribe(blob => {
      // Initieer download
      const link = document.createElement('a');
      link.href = window.URL.createObjectURL(blob);
      link.download = filename;
      link.click();
    });
  }

  showErrorDialog(message: string) {
    this.dialog.open(DetailDialogComponent, {
      data: {
        message: 'Foutmelding: ' + message,
        title: 'Foutmelding'
      }
    });
  }
}
