import { Routes } from '@angular/router';
import { LoginComponent } from './base/login/login.component';
import { BasetemplateComponent } from './base/basetemplate/basetemplate.component';
import { LogoutComponent } from './base/logout/logout.component';
import { authGuard } from './base/services/auth.guard';
import { DashboardComponent } from './app/dashboard/dashboard.component';
import { AdresComponent } from './app/adres/adres.component';
import { EigendomComponent } from './app/eigendom/eigendom.component';

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
            { path: 'eigendom/add', component: EigendomComponent },
            { path: 'eigendom/edit/:id', component: EigendomComponent },
            { path: 'adres/add/:id', component: AdresComponent },
            { path: 'adres/edit/:id', component: AdresComponent },
        ]
    },
];
