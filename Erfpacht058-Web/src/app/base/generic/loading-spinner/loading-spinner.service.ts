import { Injectable, NgZone } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingSpinnerService {

  private isLoadingSubject = new BehaviorSubject<boolean>(false);

  constructor(private ngZone: NgZone) { }

  get isLoading$(): Observable<boolean> {
    return this.isLoadingSubject.asObservable();
  }

  show(): void {
    this.ngZone.run(() => this.isLoadingSubject.next(true));
  }

  hide(): void {
    this.ngZone.run(() => this.isLoadingSubject.next(false));
  }
}
