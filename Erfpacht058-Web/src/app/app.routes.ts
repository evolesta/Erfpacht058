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
import { UsermanagementComponent } from './app/usermanagement/usermanagement.component';
import { adminGuard } from './base/services/admin.guard';
import { ModUserComponent } from './app/usermanagement/mod-user/mod-user.component';
import { RapportageComponent } from './app/rapportage/rapportage.component';
import { SjablonenComponent } from './app/rapportage/sjablonen/sjablonen.component';
import { ModSjablonenComponent } from './app/rapportage/sjablonen/mod-sjablonen/mod-sjablonen.component';
import { AddRapportageComponent } from './app/rapportage/add-rapportage/add-rapportage.component';
import { ImportComponent } from './app/import/import.component';
import { VertaaltabellenComponent } from './app/import/vertaaltabellen/vertaaltabellen.component';
import { ModVertaaltabelComponent } from './app/import/vertaaltabellen/mod-vertaaltabel/mod-vertaaltabel.component';
import { AddImportComponent } from './app/import/add-import/add-import.component';
import { FacturatieComponent } from './app/facturatie/facturatie.component';
import { AddFacturatieComponent } from './app/facturatie/add-facturatie/add-facturatie.component';

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
            { path: 'rapportage', component: RapportageComponent },
            { path: 'rapportage/add', component: AddRapportageComponent },
            { path: 'facturatie', component: FacturatieComponent },
            { path: 'facturatie/add', component: AddFacturatieComponent },
            { path: 'rapportage/add', component: AddRapportageComponent },
            { path: 'rapportage/sjablonen', component: SjablonenComponent, canActivate: [adminGuard]},
            { path: 'rapportage/sjablonen/add', component: ModSjablonenComponent, canActivate: [adminGuard]},
            { path: 'rapportage/sjablonen/edit/:id', component: ModSjablonenComponent, canActivate: [adminGuard]},
            { path: 'import', component: ImportComponent, canActivate: [adminGuard]},
            { path: 'import/add', component: AddImportComponent, canActivate: [adminGuard]},
            { path: 'import/vertaaltabellen', component: VertaaltabellenComponent, canActivate: [adminGuard]},
            { path: 'import/vertaaltabellen/add', component: ModVertaaltabelComponent, canActivate: [adminGuard]},
            { path: 'import/vertaaltabellen/edit/:id', component: ModVertaaltabelComponent, canActivate: [adminGuard]},
            { path: 'usermanagement', component: UsermanagementComponent, canActivate: [adminGuard] },
            { path: 'usermanagement/add', component: ModUserComponent, canActivate: [adminGuard] },
            { path: 'usermanagement/edit/:id', component: ModUserComponent, canActivate: [adminGuard] },
        ]
    },
];
