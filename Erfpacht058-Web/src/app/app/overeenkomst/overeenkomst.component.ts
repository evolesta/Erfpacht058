import { Component, OnInit, ViewChild, viewChild } from '@angular/core';
import { HttpHelperService } from '../../base/services/http-helper.service';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router, RouterModule } from '@angular/router';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { DetailDialogComponent } from '../../base/generic/detail-dialog/detail-dialog.component';
import { DeleteDialogComponent } from '../../base/generic/delete-dialog/delete-dialog.component';

@Component({
  selector: 'app-overeenkomst',
  standalone: true,
  imports: [MatTableModule, MatButtonModule, MatIconModule, RouterModule, DatePipe, CurrencyPipe, MatMenuModule, MatPaginatorModule, MatSortModule, 
    MatFormFieldModule, MatInputModule, MatDialogModule],
  templateUrl: './overeenkomst.component.html',
  styleUrl: './overeenkomst.component.css'
})
export class OvereenkomstComponent implements OnInit {

  overeenkomstTable = new MatTableDataSource<any>;
  overeenkomstColumns: string[] = ['dossiernummer', 'ingangsdatum', 'einddatum', 'grondwaarde', 'rentepercentage', 'bedrag', 'frequentie', 'periode', 'options'];
  frequentieOvereenkomsten = {
    0: 'Maandelijks', 1: 'Halfjaarlijks', 2: 'Jaarlijks'
  };
  perioden = {
    0: 'Juni', 1: 'December'
  }

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private http: HttpHelperService,
    private dialog: MatDialog,
    private router: Router) {}

  ngOnInit(): void {
      this.initOvereenkomsten();
  }

  initOvereenkomsten(): void {
    this.http.get('/overeenkomst').subscribe(resp => {
      const response:any = resp.body;
      this.overeenkomstTable = new MatTableDataSource(response);

      this.overeenkomstTable.paginator = this.paginator;
      this.overeenkomstTable.sort = this.sort;
    });
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.overeenkomstTable.filter = filterValue.trim().toLowerCase();

    if (this.overeenkomstTable.paginator) {
      this.overeenkomstTable.paginator.firstPage();
    }
  }

  openBekijkOvereenkomstDialog(id): void {
    this.dialog.open(DetailDialogComponent, {
      data: {
        title: 'Overeenkomst details',
        endpoint: '/overeenkomst/' + id,
        dataRows: ['dossiernummer', 'ingangsdatum', 'einddatum', 'grondwaarde', 'datumAkte', 'rentepercentage'],
        displayValues: ['Dossiernummer', 'Ingangsdatum', 'Einddatum', 'Grondwaarde', 'Datum akte', 'Rentepercentage'],
      }
    });
  }

  openVerwijderOvereenkomstDialog(id): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent);
    dialogRef.afterClosed().subscribe(result => {
      if (result.delete) {
        this.http.delete('/overeenkomst/' + id).subscribe(resp => {
          this.router.navigateByUrl('app');
        });
      }
    });
  }
}
