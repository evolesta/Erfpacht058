import { CanActivateFn } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

export const adminGuard: CanActivateFn = (route, state) => {
  // Controleer of gebruiker lid van de beheerders rol
  const token = localStorage.getItem('token');
  const decodedToken:any = jwtDecode(token);

  return decodedToken.Role == "Beheerder";
};
