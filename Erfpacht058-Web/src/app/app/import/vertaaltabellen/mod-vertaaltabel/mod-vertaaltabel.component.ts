import { Component, OnInit } from '@angular/core';
import { HttpHelperService } from '../../../../base/services/http-helper.service';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { HelperService } from '../../../../base/services/helper.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-mod-vertaaltabel',
  standalone: true,
  imports: [MatIconModule, MatButtonModule, MatInputModule, MatFormFieldModule, ReactiveFormsModule, MatSelectModule, CommonModule],
  templateUrl: './mod-vertaaltabel.component.html',
  styleUrl: './mod-vertaaltabel.component.css'
})
export class ModVertaaltabelComponent implements OnInit {

  modTabelForm = new FormGroup({
    maker: new FormControl(this.helper.naamGebruiker(), Validators.required),
    model: new FormControl('', Validators.required),
    naam: new FormControl('', Validators.required),
    translations: this.formbuilder.array([])
  });

  edit: boolean;
  tabelId: string;
  modellen: any;
  get translationDataCollection(): FormArray {
    return this.modTabelForm.get('translations') as FormArray;
  }
  gefilterdeModel: any = '';
  
  constructor(private http: HttpHelperService,
    private router: Router,
    private route: ActivatedRoute,
    private formbuilder: FormBuilder,
    private helper: HelperService) {}

  ngOnInit(): void {
    this.getModellen();  
    
    if (this.router.url.includes('edit')) {
        this.edit = true;
        this.tabelId = this.route.snapshot.paramMap.get('id');
        this.getTabel();
      }
      else {
        this.addEmptyTranslation();
      }
  }

  modTabel(): void {
    if (this.edit)
      this.editTabel();
    else
      this.addTabel();
  }

  // Verkrijg de vertaaltabel data en plaats in formGroup
  getTabel(): void {
    this.http.get('/translatemodel/' + this.tabelId).subscribe(resp => {
      const response:any = resp.body;
      this.modTabelForm.patchValue(response);

      // Doorloop alle translations en plaats in de formArray
      for (let i = 0; i < response.translations.length; i++) {
        const translationData = this.formbuilder.group({
          CSVColummnName: new FormControl(response.translations[i].csvColummnName, Validators.required),
        	ModelColumnName: new FormControl(response.translations[i].modelColumnName, Validators.required),
          id: new FormControl(response.translations[i].id, Validators.required)
        });

        this.translationDataCollection.push(translationData);
      }

      // Filter het datamodel
      this.filterModellen(response.model);
    });
  }

  getModellen(): void {
    this.http.get('/template/models').subscribe(resp => {
      const response:any = resp.body;
      this.modellen = response;
    });
  }

  addTabel(): void {
    if (this.modTabelForm.status == "VALID") {
      this.http.post('/translatemodel', this.modTabelForm.value).subscribe(resp => {
        this.router.navigateByUrl('app/import/vertaaltabellen');
      });
    }
  }

  editTabel(): void {
    this.http.put('/translatemodel/' + this.tabelId, this.modTabelForm.value).subscribe(resp => {
      this.router.navigateByUrl('app/import/vertaaltabellen');
    });
  }

  // ++ Vertalingen array functies
  // Voeg een nieuw leeg item toe aan de formbuilder array
  addEmptyTranslation(): void {
    const translation = this.formbuilder.group({
      CSVColummnName: new FormControl('', Validators.required),
      ModelColumnName: new FormControl('', Validators.required),
    });

    this.translationDataCollection.push(translation);
  }

  // Filter de collectie met modellen
  filterModellen(event): void {
    const filter = this.modellen.filter(model => model.tableName === event);
    this.gefilterdeModel = filter[0];
  }

  deleteTranslation(id): void {
    this.translationDataCollection.removeAt(id);
  }
}
