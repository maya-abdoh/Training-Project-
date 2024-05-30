import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GeofenceService {

  private baseUrl = 'https://localhost:7276/api/Geofences';

  constructor(private http: HttpClient) { }

  getGeofences(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}`);
  }

}
