import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { HttpHelperService } from '../../../base/services/http-helper.service';
import { MatTable, MatTableDataSource, MatTableModule } from '@angular/material/table';
import { DatePipe } from '@angular/common';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialog } from '@angular/material/dialog';
import { DeleteDialogComponent } from '../../../base/generic/delete-dialog/delete-dialog.component';

@Component({
  selector: 'app-vertaaltabellen',
  standalone: true,
  imports: [MatButtonModule, MatIconModule, RouterModule, MatTableModule, DatePipe, MatMenuModule],
  templateUrl: './vertaaltabellen.component.html',
  styleUrl: './vertaaltabellen.component.css'
})
export class VertaaltabellenComponent implements OnInit {

  translationTable: MatTableDataSource<any>;
  displayedColumns: string[] = ['naam', 'model', 'maker', 'aanmaakDatum', 'wijzigingsDatum', 'options'];

  constructor(private http: HttpHelperService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
      this.getTables();
  }

  getTables(): void {
    this.http.get('/translatemodel').subscribe(resp => {  
      const response:any = resp.body;
      this.translationTable = new MatTableDataSource(response);
    });
  }

  removeTable(id): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.delete) {
        this.http.delete('/translatemodel/' + id).subscribe(resp => {
          this.getTables();
        });
      }
    });
  }
}
