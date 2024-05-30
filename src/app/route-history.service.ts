import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RouteHistoryService {
  private apiUrl = 'https://localhost:7276/api/Routehistories';

  constructor(private http: HttpClient) {}

  addRouteHistory(routeHistory: any): Observable<any> {
    const payload = {
      DicOfDic: {
        "DATA": {
          "vehicleID": routeHistory.vehicleID,
          "vehicleDirection": routeHistory.vehicleDirection,
          "status": routeHistory.status,
          "gpsTime": routeHistory.gpsTime,
          "address": routeHistory.address,
          "latitude": routeHistory.latitude,
          "longitude": routeHistory.longitude,
          "gpsSpeed": routeHistory.gpsSpeed
        }
      }
    };
    return this.http.post<any>(this.apiUrl, payload);
  }
  getRouteHistory(vehicleId: number): Observable<any> {
    const url = `${this.apiUrl}/${vehicleId}`;
    console.log(`Fetching route history from: ${url}`);
    return this.http.get<any>(url);
  }
  
}
