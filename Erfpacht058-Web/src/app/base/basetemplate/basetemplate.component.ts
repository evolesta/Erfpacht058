import { Component, OnInit } from '@angular/core';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatListModule} from '@angular/material/list';
import {MatIconModule} from '@angular/material/icon';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatTooltipModule} from '@angular/material/tooltip';
import {MatButtonModule} from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-basetemplate',
  standalone: true,
  imports: [MatSidenavModule, MatListModule, MatIconModule, MatToolbarModule, MatTooltipModule, MatButtonModule, RouterModule],
  templateUrl: './basetemplate.component.html',
  styleUrl: './basetemplate.component.css'
})
export class BasetemplateComponent implements OnInit {

  naam: string;
  inlog: string;

  constructor() {}

  ngOnInit(): void {
      this.getNaam();
  }

  // Functie om de naam van de gebruiker uit de token te verkrijgen
  getNaam(): void {
    const token = localStorage.getItem('token');
    const decodedToken:any = jwtDecode(token);

    this.naam = decodedToken.Naam;
    const loginTS = new Date(decodedToken.Login * 1000).toString();
    const datepipe = new DatePipe('en-US');
    this.inlog = datepipe.transform(loginTS, 'dd-MM-yyyy HH:mm')
  }
}
