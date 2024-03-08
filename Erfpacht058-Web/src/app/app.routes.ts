import { Routes } from '@angular/router';
import { LoginComponent } from './base/login/login.component';
import { BasetemplateComponent } from './base/basetemplate/basetemplate.component';
import { LogoutComponent } from './base/logout/logout.component';
import { authGuard } from './base/services/auth.guard';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AddEigendomComponent } from './app/eigendom/add-eigendom/add-eigendom.component';
import { AdresComponent } from './app/adres/adres.component';

export const routes: Routes = [
    // Publieke routes
    { path: '', component: LoginComponent },
    { path: 'logout', component: LogoutComponent },

    // Beveiligde routes
    { 
        path: 'app', 
        component: BasetemplateComponent,
        canActivate: [authGuard],
        children: [
            { path: '', component: DashboardComponent },
            { path: 'eigendom/add', component: AddEigendomComponent },
            { path: 'adres/add/:id', component: AdresComponent },
            { path: 'adres/edit/:id', component: AdresComponent },
        ]
    },
];
