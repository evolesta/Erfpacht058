import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { HelperService } from '../../base/services/helper.service';

@Component({
  selector: 'app-rapportage',
  standalone: true,
  imports: [MatButtonModule, MatIconModule, RouterModule, CommonModule],
  templateUrl: './rapportage.component.html',
  styleUrl: './rapportage.component.css'
})
export class RapportageComponent {

  constructor(public helper: HelperService) {}
}
