import { Inject, OnInit } from '@angular/core';
import { Component } from '@angular/core';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import {MAT_DIALOG_DATA} from '@angular/material/dialog';
import { HttpHelperService } from '../../services/http-helper.service';

@Component({
  selector: 'app-search-dialog',
  standalone: true,
  imports: [MatTableModule],
  templateUrl: './search-dialog.component.html',
  styleUrl: './search-dialog.component.css'
})
export class SearchDialogComponent implements OnInit {

  tableData: MatTableDataSource<any>; // generieke tabel data object

  constructor(@Inject(MAT_DIALOG_DATA) public data: {endpoint: string},
    private http: HttpHelperService) {}

  ngOnInit(): void {
    this.getData();
  }

  // Verkrijg data van API bij initialiseren component
  getData(): void {
    this.http.get(this.data.endpoint).subscribe(resp => {
      const response:any = resp.body;
      this.tableData = new MatTableDataSource(response);
    });
  }

}
