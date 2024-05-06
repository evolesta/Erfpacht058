import { Component, OnInit } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { HttpHelperService } from '../../../base/services/http-helper.service';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { Router, RouterModule } from '@angular/router';
import { DatePipe } from '@angular/common';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialog } from '@angular/material/dialog';
import { DeleteDialogComponent } from '../../../base/generic/delete-dialog/delete-dialog.component';

@Component({
  selector: 'app-sjablonen',
  standalone: true,
  imports: [MatTableModule, MatIconModule, MatButtonModule, RouterModule, DatePipe, MatMenuModule],
  templateUrl: './sjablonen.component.html',
  styleUrl: './sjablonen.component.css'
})
export class SjablonenComponent implements OnInit {

  templatesTable: MatTableDataSource<any>;
  templatesColumns: string[] = ['naam', 'model', 'maker', 'aanmaakDatum', 'wijzigingsDatum', 'options'];

  constructor(private http: HttpHelperService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
      this.getTemplates();
  }

  getTemplates(): void {
    this.http.get('/template').subscribe(resp => {
      const response:any = resp.body;
      this.templatesTable = new MatTableDataSource(response);
    });
  }

  openVerwijderDialog(id: string): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result.delete) {
        this.http.delete('/template/' + id).subscribe(resp => {
          this.getTemplates();
        });
      }
    });
  }
}
