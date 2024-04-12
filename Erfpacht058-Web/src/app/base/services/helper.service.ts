import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class HelperService {

  constructor() { }

  tokenValidator(token: any): boolean {
    // Controleer of de token nog geldig is
    const decodedToken = jwtDecode(token);
    const currTimestamp = Math.floor(Date.now() / 1000); // converteer huidige tijd naar seconden

    // Als de expiration timestamp op de token aanwezig is en in de toekomst is deze nog geldig
    return (decodedToken.exp != null && decodedToken.exp > currTimestamp);
  }

  isAdministrator(): boolean {
    // Verkrijg token en decode
    const token = localStorage.getItem('token');
    const decodedToken:any = jwtDecode(token);

    return decodedToken.Role == "Beheerder";
  }
}
