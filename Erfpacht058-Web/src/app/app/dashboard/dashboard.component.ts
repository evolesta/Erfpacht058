import { Component, OnInit } from '@angular/core';
import { HttpHelperService } from '../../base/services/http-helper.service';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import {MatGridListModule} from '@angular/material/grid-list';
import { SearchDialogComponent } from '../../base/generic/search-dialog/search-dialog.component';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatMenuModule} from '@angular/material/menu';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { DeleteDialogComponent } from '../../base/generic/delete-dialog/delete-dialog.component';
import { UploadDialogComponent } from '../../base/generic/upload-dialog/upload-dialog.component';
import { environment } from '../../../environments/environment';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { DetailDialogComponent } from '../../base/generic/detail-dialog/detail-dialog.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [MatButtonModule, MatIconModule, CommonModule, RouterModule, MatGridListModule, MatExpansionModule, MatMenuModule, MatTableModule, 
    MatMenuModule, MatTooltipModule, MatInputModule, MatFormFieldModule, ReactiveFormsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {

  // Globale variabelen
  eigendom: any; // Bevat geselecteerde Eigendom object
  loaded: boolean;
  eigenarenTable: MatTableDataSource<any>;
  eigenarenColumns: string[] = ['naam', 'voorletters', 'debiteurnummer', 'ingangsdatum', 'einddatum', 'options'];
  bestandenTable: MatTableDataSource<any>;
  bestandenColumns: string[] = ['naam', 'soortBestand', 'grootteInKb', 'download', 'options'];
  soortenBestanden = {
    0: "Algemeen", 1: "Notitie", 2: "Bewijsstuk", 3: "Overeenkomst", 4: "Overig"
  }
  overeenkomstenTable: MatTableDataSource<any>;
  overeenkomstenColumns: string[] = ['dossiernummer', 'ingangsdatum', 'einddatum', 'rentepercentage', 'bedrag', 'frequentie', 'options'];
  frequentieOvereenkomsten = {
    0: 'Maandelijks', 1: 'Halfjaarlijks', 2: 'Jaarlijks'
  }
  downloadEndpoint: string = environment.apiURL + '/bestand/download/';
  notities = new FormControl('');
  editNotities: boolean;

  constructor(private http: HttpHelperService,
    private dialog: MatDialog) {}

  ngOnInit(): void {
    this.initEigendom();
  }

  // +++ EIGENDOM FUNCTIES

  // Functie die informatie over het eigendom verkrijgt indien geselecteerd in localStorage
  initEigendom(): void {
    const eigendomId = localStorage.getItem('eigendomId'); // Verkrijg eigendomId uit localstorage

    // Als eigendom geselecteerd is verkrijg informatie van back-end
    if (eigendomId != null) {
      this.http.get('/eigendom/' + eigendomId).subscribe(resp => {
        // Algemene data
        const response:any = resp.body;
        this.eigendom = response;

        // Tabellen opbouwen voor many relaties
        this.eigenarenTable = new MatTableDataSource(response.eigenaar);
        this.bestandenTable = new MatTableDataSource(response.bestand);
        this.overeenkomstenTable = new MatTableDataSource(response.overeenkomst);

        // Zet de notities van het eigendom naar de FormControl
        this.notities.setValue(this.eigendom.notities);

        this.loaded = true; // voorkom undefined errors ivm async http request
      });
    }
  }

  // Openen van het zoekvenster voor een eigendom
  openZoekDialogEigendom(): void {
    const dialogRef = this.dialog.open(SearchDialogComponent, {
      data: {
        endpoint: '/eigendom',
        title: 'Eigendom',
        displayedColumns: ['id', 'relatienummer'],
        columnNames: ['Eigendomnummer', 'Relatienummer']
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.result) {
        localStorage.setItem('eigendomId', result.row.id);
        this.initEigendom();
      }
    });
  }

  // +++ ADRES FUNCTIES

  // Open het zoekvenster
  openZoekDialogAdres(): void {
    // Gebruik het gen. zoekvenster component
    const dialogRef = this.dialog.open(SearchDialogComponent, {
      data: {
        endpoint: '/eigendom/adres',
        title: 'Adres',
        displayedColumns: ['straatnaam', 'huisnummer', 'toevoeging', 'postcode', 'woonplaats'],
        columnNames: ['Straatnaam', 'Huisnummer', 'Toevoeging', 'Postcode', 'Woonplaats']
      }
    });

    // Update geselecteerde object alleen wanneer het venster is gesloten na het maken van een keuze
    dialogRef.afterClosed().subscribe(result => {
      if (result.result) {
        localStorage.setItem('eigendomId', result.row.eigendomId);
        this.initEigendom();
      }
    });
  }

  // +++ EIGENAAR functies

  // Zoek de dialog om een eigenaar te koppelen aan een eigendom
  openZoekDialogEignKopp(): void {
    const dialogRef = this.dialog.open(SearchDialogComponent, {
      data: {
        endpoint: '/eigenaar',
        title: 'Eigenaar',
        displayedColumns: ['naam', 'voorletters', 'debiteurnummer'],
        columnNames: ['Naam', 'Voorletters', 'Debiteurnummer']
      }
    });

    // Koppel de eigenaar aan het eigendom
    dialogRef.afterClosed().subscribe(result => {
      // Alleen verwerken als er een rij is aangeklikt
      if (result.result) {
        // Aanroepen van koppel endpoint om eigenaar te koppelen aan eigendom
        const eigenaarId = result.row.id;
        this.http.put('/eigendom/eigenaar/' + this.eigendom.id + '/' + eigenaarId, null).subscribe(resp => {
          this.initEigendom();
        });
      }
    });
  }

  // Ontkoppel een eigenaar vanuit het eigendom
  openOntkDialogEign(eigenaarId): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent);
    dialogRef.afterClosed().subscribe(result => {
      if (result.delete) {
        this.http.delete('/eigendom/eigenaar/' + this.eigendom.id + '/' + eigenaarId).subscribe(resp => {
          this.initEigendom();
        });
      }
    });
  }

  openDetailsEign(eigenaarId): void {
    this.dialog.open(DetailDialogComponent, {
      data: {
        title: 'Eigenaar details',
        endpoint: '/eigenaar/' + eigenaarId,
        dataRows: ['naam', 'voornamen', 'voorletters', 'straatnaam', 'huisnummer', 'toevoeging', 'postcode', 'woonplaats', 'debiteurnummer'],
        displayValues: ['Naam', 'Voornamen', 'Voorletters', 'Straatnaam', 'Huisnummer', 'Toevoeging', 'Postcode', 'Woonplaats', 'Debiteurnummer'],
      }
    });
  }

  // +++ KADASTER FUNCTIES

  // Aanroepen van de synchronisatie in de back-end met het Kadaster
  syncKadaster(): void {
    this.http.post('/kadaster/sync/' + this.eigendom.kadaster.id, '').subscribe(resp => {
      this.initEigendom();
    });
  }

  // ++ BESTANDEN FUNCTIES

  // Upload een bestand dialog
  openUploadFileDialog(): void {
    const dialRef = this.dialog.open(UploadDialogComponent, {
      data: {
        id: this.eigendom.id
      }
    });
    dialRef.afterClosed().subscribe(result => {
      this.initEigendom();
    });
  }

  // Verwijder een bestaand bestand met bevestiging
  openVerwijderFileDialog(id): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent);
    dialogRef.afterClosed().subscribe(result => {
      if (result.delete) {
        this.http.delete('/bestand/' + id).subscribe(resp => {
          this.initEigendom();
        });
      }
    });
  }

  // +++ NOTITIES FUNCTIES

  // Wijzig de notities naar het object in de database
  updateNotes(): void {
    this.eigendom.notities = this.notities.value;
    this.http.put('/eigendom/' + this.eigendom.id, this.eigendom).subscribe(resp => {
      this.initEigendom();
      this.editNotities = false;
    });
  }

  // ++ OVEREENKOMST FUNCTIES

  // Koppel een overeenkomst aan een eigendom
  openKoppelOvereenkomstDialog(id): void {
    const dialogRef = this.dialog.open(SearchDialogComponent, {
      data: {
        endpoint: '/overeenkomst',
        title: 'Overeenkomst',
        displayedColumns: ['dossiernummer', 'ingangsdatum'],
        columnNames: ['Dossiernummer', 'Ingangsdatum']
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      // Alleen verwerken als er een rij is aangeklikt
      if (result.result) {
        // Aanroepen van koppel endpoint om eigenaar te koppelen aan eigendom
        const overeenkomstId = result.row.id;
        this.http.put('/eigendom/overeenkomst/' + this.eigendom.id + '/' + overeenkomstId, null).subscribe(resp => {
          this.initEigendom();
        });
      }
    });
  }

  // Open detail venster voor een overeenkomst
  openBekijkOvereenkomstDialog(id): void {
    this.dialog.open(DetailDialogComponent, {
      data: {
        title: 'Overeenkomst details',
        endpoint: '/overeenkomst/' + id,
        dataRows: ['dossiernummer', 'ingangsdatum', 'einddatum', 'grondwaarde', 'datumAkte', 'rentepercentage'],
        displayValues: ['Dossiernummer', 'Ingangsdatum', 'Einddatum', 'Grondwaarde', 'Datum akte', 'Rentepercentage'],
      }
    })
  }

  // Verwijder een overeenkomst
  openVerwijderOvereenkomstDialog(id): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result.delete) {
        this.http.delete('/overeenkomst/' + id).subscribe(resp => {
          this.initEigendom();
        });
      }
    });
  }
}
