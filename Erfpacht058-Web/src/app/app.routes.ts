import { Routes } from '@angular/router';
import { LoginComponent } from './base/login/login.component';
import { BasetemplateComponent } from './base/basetemplate/basetemplate.component';
import { LogoutComponent } from './base/logout/logout.component';
import { authGuard } from './base/services/auth.guard';
import { DashboardComponent } from './app/dashboard/dashboard.component';
import { AdresComponent } from './app/adres/adres.component';
import { EigendomComponent } from './app/eigendom/eigendom.component';
import { EigenaarComponent } from './app/eigenaar/eigenaar.component';
import { ListEigenaarComponent } from './app/eigenaar/list-eigenaar/list-eigenaar.component';
import { HerzieningComponent } from './app/herziening/herziening.component';
import { KadasterComponent } from './app/kadaster/kadaster.component';
import { BestandComponent } from './app/bestand/bestand.component';
import { OvereenkomstComponent } from './app/overeenkomst/overeenkomst.component';
import { ModOvereenkomstComponent } from './app/overeenkomst/mod-overeenkomst/mod-overeenkomst.component';
import { LoadingSpinnerComponent } from './base/generic/loading-spinner/loading-spinner.component';

export const routes: Routes = [
    // Publieke routes
    { path: '', component: LoginComponent },
    { path: 'logout', component: LogoutComponent },
    { path: 'test', component: LoadingSpinnerComponent },

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
            { path: 'eigenaar', component: ListEigenaarComponent },
            { path: 'eigenaar/add/:id', component: EigenaarComponent },
            { path: 'eigenaar/edit/:id', component: EigenaarComponent },
            { path: 'herziening/add/:id', component: HerzieningComponent },
            { path: 'herziening/edit/:id', component: HerzieningComponent },
            { path: 'kadaster/add/:id', component: KadasterComponent },
            { path: 'kadaster/edit/:id', component: KadasterComponent },
            { path: 'bestand/edit/:id', component: BestandComponent },
            { path: 'overeenkomst', component: OvereenkomstComponent },
            { path: 'overeenkomst/add/:id', component: ModOvereenkomstComponent },
            { path: 'overeenkomst/edit/:id', component: ModOvereenkomstComponent },
        ]
    },
];
