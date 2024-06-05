import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { LoadingSpinnerService } from './loading-spinner.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-loading-spinner',
  standalone: true,
  imports: [MatProgressSpinnerModule, CommonModule],
  templateUrl: './loading-spinner.component.html',
  styleUrl: './loading-spinner.component.css'
})
export class LoadingSpinnerComponent implements OnInit {

  isLoading: boolean = false;

  constructor(public spinner: LoadingSpinnerService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
      this.spinner.isLoading$.subscribe((loading: boolean) => {
        this.isLoading = loading;
        this.cdr.detectChanges();
      });
  }
}
