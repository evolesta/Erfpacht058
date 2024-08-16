import { DialogModule } from '@angular/cdk/dialog';
import {} from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { HttpHelperService } from '../../../base/services/http-helper.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-reset-password-dialog',
  standalone: true,
  imports: [MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, ReactiveFormsModule],
  templateUrl: './reset-password-dialog.component.html',
  styleUrl: './reset-password-dialog.component.css'
})
export class ResetPasswordDialogComponent {

  resetPasswordForm = new FormGroup({
    wachtwoord: new FormControl('', Validators.required),
    wachtwoord2: new FormControl('', Validators.required)
  });

  constructor(private http: HttpHelperService,
    @Inject(MAT_DIALOG_DATA) public data: {id: string},
    private dialog: MatDialogRef<ResetPasswordDialogComponent>) {  }

  resetPassword(): void {
    if (this.wachtwoordMatch(this.resetPasswordForm.controls.wachtwoord.value, this.resetPasswordForm.controls.wachtwoord2.value)) {
        this.http.put('/gebruikers/wachtwoord/' + this.data.id, this.resetPasswordForm.value).subscribe(resp => {
          this.dialog.close({success: true});
        });
      }
  }

  // + Helper functies

  // Functie die controleert of beiden wachtwoorden overeenkomst
  wachtwoordMatch(wachtwoord1, wachtwoord2): boolean {
    return wachtwoord1 === wachtwoord2;
  }
}
