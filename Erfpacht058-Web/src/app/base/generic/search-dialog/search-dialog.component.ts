import { ChangeDetectorRef, Inject, OnInit, ViewChild } from '@angular/core';
import { Component } from '@angular/core';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import {MAT_DIALOG_DATA, MatDialogModule, MatDialogRef} from '@angular/material/dialog';
import { HttpHelperService } from '../../services/http-helper.service';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-search-dialog',
  standalone: true,
  imports: [MatTableModule, MatDialogModule, MatButtonModule, MatIconModule, CommonModule,
     MatSortModule, MatPaginatorModule, MatFormFieldModule, MatInputModule, MatDialogModule],
  templateUrl: './search-dialog.component.html',
  styleUrl: './search-dialog.component.css'
})
export class SearchDialogComponent implements OnInit {

  tableData: MatTableDataSource<any>; // generieke tabel data object
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(@Inject(MAT_DIALOG_DATA) public data: {endpoint: string, title: string, displayedColumns: string[], columnNames: string[]},
    private http: HttpHelperService,
    private dialogRef: MatDialogRef<SearchDialogComponent>) {}

  ngOnInit(): void {
    this.getData();
  }

  // Verkrijg data van API bij initialiseren component
  getData(): void {
    this.http.get(this.data.endpoint).subscribe(resp => {
      const response:any = resp.body;
      this.tableData = new MatTableDataSource(response);
      this.tableData.paginator = this.paginator;
      this.tableData.sort = this.sort;
    });
  }

  // Functie om data te filteren
  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.tableData.filter = filterValue.trim().toLowerCase();

    if (this.tableData.paginator) {
      this.tableData.paginator.firstPage();
    }
  }

  selecteerKeuze(row: string): void { 
    this.dialogRef.close({
      result: true,
      row: row
    });
  }

}
