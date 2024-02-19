import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HttpHelperService {

  constructor(private http: HttpClient) { }

  public get(endpoint: string) {
    return this.http.get(environment.apiURL + endpoint, { observe: 'response' });
  }

  public post(endpoint: string, body: any) {
    return this.http.post(environment.apiURL + endpoint, body, { observe: 'response' });
  }

  public put(endpoint: string, body: any) {
    return this.http.put(environment.apiURL + endpoint, body, { observe: 'response' });
  }

  public delete(endpoint: string) {
    return this.http.delete(environment.apiURL + endpoint, { observe: 'response' });
  }
}
