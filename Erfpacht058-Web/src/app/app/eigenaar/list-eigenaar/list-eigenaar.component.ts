import { Component, OnInit, ViewChild, viewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { HttpHelperService } from '../../../base/services/http-helper.service';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { CommonModule, DatePipe } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { DeleteDialogComponent } from '../../../base/generic/delete-dialog/delete-dialog.component';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-list-eigenaar',
  standalone: true,
  imports: [MatTableModule, MatButtonModule, MatIconModule, MatMenuModule, DatePipe, CommonModule, RouterModule, MatDialogModule, MatPaginator, MatInputModule],
  templateUrl: './list-eigenaar.component.html',
  styleUrl: './list-eigenaar.component.css'
})
export class ListEigenaarComponent implements OnInit {

  eigenaarData: MatTableDataSource<any>;
  displayedColumns: string[] = ['naam', 'voorletters', 'debiteurnummer', 'ingangsdatum', 'einddatum', 'opties'];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private http: HttpHelperService,
    private router: Router,
    private dialog: MatDialog) {}

  ngOnInit(): void {
      this.initEigenaar();
  }

  initEigenaar(): void {
    this.http.get("/eigenaar").subscribe(resp => {
      const response:any = resp.body;
      this.eigenaarData = new MatTableDataSource(response);
      this.eigenaarData.paginator = this.paginator;
      this.eigenaarData.sort = this.sort;
    });
  }

  openVerwijderEigenaarDialog(id): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent);
    dialogRef.afterClosed().subscribe(result => {
      // Alleen verwijderen wanneer verwijder knop is aangeklikt
      if (result && result.delete) {
        	this.http.delete('/eigenaar/' + id).subscribe(resp => {
            this.initEigenaar();
          });
      }
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.eigenaarData.filter = filterValue.trim().toLowerCase();

    if (this.eigenaarData.paginator) {
      this.eigenaarData.paginator.firstPage();
    }
  }
}
