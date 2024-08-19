import { Injectable, OnInit } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

interface JwtPayload {
  exp?: number;
  Role?: string;
  Naam?: string;
}

@Injectable({
  providedIn: 'root'
})
export class HelperService {

  tokenValidator(): boolean {
    const token = localStorage.getItem('token');
    if (!token) return false;
    const decodedToken = jwtDecode<JwtPayload>(token);

    // Controleer of de token nog geldig is
    const currTimestamp = Math.floor(Date.now() / 1000); // converteer huidige tijd naar seconden

    // Als de expiration timestamp op de token aanwezig is en in de toekomst is deze nog geldig
    return (decodedToken.exp != null && decodedToken.exp > currTimestamp);
  }

  isAdministrator(): boolean {
    try {
      const token = localStorage.getItem('token');
      const decodedToken = jwtDecode<JwtPayload>(token);

      return decodedToken.Role == "Beheerder";
    } catch (error) {
      console.error('Failed to decode token:', error);
      return false;
    }
  }

  naamGebruiker(): string {
    try {
      const token = localStorage.getItem('token');
      const decodedToken = jwtDecode<JwtPayload>(token);

      return decodedToken?.Naam;
    } catch (error) {
      console.error('Failed to decode token: ', error);
      return null;
    }
  }
}
