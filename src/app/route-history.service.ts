import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

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
    return this.http.post<any>(this.apiUrl, payload).pipe(
      catchError(this.handleError)
    );
  }

  getRouteHistory(vehicleId: number): Observable<any> {
    const url = `${this.apiUrl}/${vehicleId}`;
    console.log(`Fetching route history from: ${url}`);
    return this.http.get<any>(url).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Unknown error!';
    if (error.error instanceof ErrorEvent) {
      // Client-side errors
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side errors
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.error(errorMessage);
    return throwError(errorMessage);
  }
}
