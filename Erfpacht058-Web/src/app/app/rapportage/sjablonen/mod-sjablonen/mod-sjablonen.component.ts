import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpHelperService } from '../../../../base/services/http-helper.service';
import { Form, FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { HelperService } from '../../../../base/services/helper.service';

@Component({
  selector: 'app-mod-sjablonen',
  standalone: true,
  imports: [ReactiveFormsModule, MatInputModule, MatFormFieldModule, MatIconModule, MatButtonModule, MatSelectModule, CommonModule],
  templateUrl: './mod-sjablonen.component.html',
  styleUrl: './mod-sjablonen.component.css'
})
export class ModSjablonenComponent implements OnInit {

  // Genereer een FormGroup met twee dynamische FormBuilder arrays die door de gebruiker gevuld kunnen worden
  templateForm = new FormGroup({
    naam: new FormControl('', Validators.required),
    maker: new FormControl(this.helper.naamGebruiker()),
    model: new FormControl('', Validators.required),
    rapportData: this.formbuilder.array([]),
    filters: this.formbuilder.array([])
  });

  edit: boolean;
  templateId: string;
  modelOpties: any;
  gefilterdModel: any = '';
  operatorChoices: string[] = ['Gelijk aan (==)', 'Niet gelijk aan (!=)', 'Groter dan (>)', 'Kleiner dan (<)', 'Groter of gelijk (>=)', 'Kleiner of gelijk (<=)'];
  get rapportDataCollection(): FormArray {
    return this.templateForm.get('rapportData') as FormArray;
  }
  get filterDataCollection(): FormArray {
    return this.templateForm.get('filters') as FormArray;
  }


  constructor(private route: ActivatedRoute,
    private router: Router,
    private http: HttpHelperService,
    private formbuilder: FormBuilder,
    private helper: HelperService) {}

  ngOnInit(): void {
    this.getModellen();
    
    if (this.router.url.includes('edit')) {
      this.edit = true;
      this.templateId = this.route.snapshot.paramMap.get('id');
      this.getTemplate();
    }
    else {
      this.addEmptyRapportData();
    }
  }

  // Verkrijg het bestaande template van de API
  getTemplate(): void {
    	this.http.get('/template/' + this.templateId).subscribe(resp => {
        this.templateForm.patchValue(resp.body);

        // maak een nieuwe FormControl aan voor elke rapportData rij
        const data:any = resp.body;
        for (let i = 0; i < data.rapportData.length; i++) {
          const rapportData = this.formbuilder.group({
            key: [data.rapportData[i].key, Validators.required],
            naam: [data.rapportData[i].naam, Validators.required],
            id: [data.rapportData[i].id]
          });
          this.rapportDataCollection.push(rapportData); // voeg toe aan array
        }

        // maak een nieuwe FormControl aan voor elke filter rij
        for (let i = 0; i < data.filters.length; i++) {
          const filterData = this.formbuilder.group({
            key: new FormControl(data.filters[i].key, Validators.required),
            operation: new FormControl(data.filters[i].operation, Validators.required),
            value: new FormControl(data.filters[i].value, Validators.required),
            id: new FormControl(data.filters[i].id, Validators.required),
          });
          this.filterDataCollection.push(filterData); // Voeg toe aan array
        }

        this.filterModellen(data.model); // Filter het datamodel
      });
  }

  modSjabloon(): void {
    if(this.edit) this.editSjabloon();
    else this.addSjabloon();
  }

  addSjabloon(): void {
    this.http.post('/template', this.templateForm.value).subscribe(resp => {
      this.router.navigateByUrl('app/rapportage/sjablonen');
    });
  }

  editSjabloon(): void {
    this.http.put('/template/' + this.templateId, this.templateForm.value).subscribe(resp => {
      this.router.navigateByUrl('app/rapportage/sjablonen');
    });
  }

  // ++ Form Builder Functies
  addEmptyRapportData(): void {
    // Stel een nieuw FormGroup item op en voeg toe aan de array
    const rapportData = this.formbuilder.group({
      key: ['', Validators.required],
      naam: ['', Validators.required],
    });
    this.rapportDataCollection.push(rapportData); // voeg toe aan array
  }

  addEmptyFilterData(): void {
    const filterData = this.formbuilder.group({
      key: new FormControl('', Validators.required),
      operation: new FormControl('', Validators.required),
      value: new FormControl('', Validators.required)
    });
    this.filterDataCollection.push(filterData); // Voeg toe aan array
  }

  removeFilterData(id): void {
    this.filterDataCollection.removeAt(id);
  }

  // Verwijder een item uit de array a.d.h.v. de index
  removeRapportData(id): void {
    this.rapportDataCollection.removeAt(id);
  }

  // Verkrijg bestaande modellen van de API waarop een rapportage gemaakt kan worden
  getModellen(): void {
    this.http.get('/template/models').subscribe(resp => {
      this.modelOpties = resp.body;
    });
  }

  // Nadat er een model is gekozen, filter door de modelstructuur en kies de bijbehorende velden
  filterModellen(event): void {
    const filter = this.modelOpties.filter(model => model.tableName === event);
    this.gefilterdModel = filter[0];
  }
}
