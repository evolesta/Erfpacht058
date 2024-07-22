import { Injectable, OnInit } from '@angular/core';
import { JwtPayload, jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class HelperService {

  constructor() { 
    const token = localStorage.getItem('token');
    if (token != null) this.decodedToken = jwtDecode(token);
  }

  decodedToken: JwtPayload;

  tokenValidator(): boolean {
    // Wanneer er geen token aanwezig - geef False terug
    if (this.decodedToken == null)
      return false;

    // Controleer of de token nog geldig is
    const currTimestamp = Math.floor(Date.now() / 1000); // converteer huidige tijd naar seconden

    // Als de expiration timestamp op de token aanwezig is en in de toekomst is deze nog geldig
    return (this.decodedToken.exp != null && this.decodedToken.exp > currTimestamp);
  }

  isAdministrator(): boolean {
    const decodedToken:any = this.decodedToken;
    return decodedToken.Role == "Beheerder";
  }

  naamGebruiker(): string {
    const decodedToken:any = this.decodedToken;
    return decodedToken.Naam;
  }
}
