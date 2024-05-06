import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { HttpHelperService } from '../../services/http-helper.service';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-detail-dialog',
  standalone: true,
  imports: [MatDialogModule, CommonModule, MatIconModule],
  templateUrl: './detail-dialog.component.html',
  styleUrl: './detail-dialog.component.css'
})
export class DetailDialogComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: {title: string, dataRows: string[], displayValues: string[], endpoint: string, message: string},
    private http: HttpHelperService) {}

  showData: {};
  loaded: boolean; // undefined errors voorkomen ivm async http request voor data

  ngOnInit(): void {
      this.getData();
  }

  getData(): void {
    if (this.data.endpoint != null) {
      this.http.get(this.data.endpoint).subscribe(resp => {
        const response:any = resp.body;
        this.showData = response;
        this.loaded = true;
      });
    }
  }

}
