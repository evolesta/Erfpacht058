import { Component, OnInit } from '@angular/core';
import { HttpHelperService } from '../../base/services/http-helper.service';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import {MatSnackBar, MatSnackBarModule} from '@angular/material/snack-bar';
import { DeleteDialogComponent } from '../../base/generic/delete-dialog/delete-dialog.component';
import { ResetPasswordDialogComponent } from './reset-password-dialog/reset-password-dialog.component';

@Component({
  selector: 'app-usermanagement',
  standalone: true,
  imports: [MatTableModule, CommonModule, MatIconModule, MatMenuModule, MatButtonModule, RouterModule, MatSnackBarModule],
  templateUrl: './usermanagement.component.html',
  styleUrl: './usermanagement.component.css'
})
export class UsermanagementComponent implements OnInit {

  displayedColumns: string[] = ['naam', 'voornamen', 'emailadres', 'rol', 'actief', 'acties'];
  usersTable: MatTableDataSource<any>;
  rol = {
    0: 'Gebruiker',
    1: 'Beheerder'
  }

  constructor(private http: HttpHelperService,
    private dialog: MatDialog,
    private snackbar: MatSnackBar) {}

  ngOnInit(): void {
      this.initUsers();
  }

  initUsers(): void {
    this.http.get('/gebruikers').subscribe(resp => {
      const response:any = resp.body;
      this.usersTable = new MatTableDataSource(response);
    });
  }

  openVerwijderGebruikerDialog(id): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result.delete) {
        this.http.delete('/gebruikers/' + id).subscribe(resp => {
          this.initUsers();
        });
      }
    });
  }

  openResetWachtwoordDialog(id): void {
    const dialogRef = this.dialog.open(ResetPasswordDialogComponent, {
      data: {id: id}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.success) {
        this.snackbar.open('Wachtwoord succesvol gereset', '', {duration: 4000});
      }
    });
  }
}
