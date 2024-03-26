import { Component, OnInit } from '@angular/core';
import { HttpHelperService } from '../../base/services/http-helper.service';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-overeenkomst',
  standalone: true,
  imports: [MatTableModule, MatButtonModule, MatIconModule, RouterModule],
  templateUrl: './overeenkomst.component.html',
  styleUrl: './overeenkomst.component.css'
})
export class OvereenkomstComponent implements OnInit {

  overeenkomstTable = new MatTableDataSource<any>;
  overeenkomstColumns: string[] = ['dossiernummer', 'ingangsdatum', 'einddatum', 'grondwaarde', 'rentepercentage'];

  constructor(private http: HttpHelperService) {}

  ngOnInit(): void {
      this.initOvereenkomsten();
  }

  initOvereenkomsten(): void {
    this.http.get('/overeenkomst').subscribe(resp => {
      const response:any = resp.body;
      this.overeenkomstTable = new MatTableDataSource(response);
    });
  }
}
