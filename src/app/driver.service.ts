import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DriverService {
  private apiUrl = 'https://localhost:7276/api/Drivers'; 

  constructor(private http: HttpClient) {}

  getDrivers(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }

  addDriver(driver: any): Observable<any> {
    const payload = {
      DicOfDic: {
        DATA: {
          driverName: driver.driverName,
          phoneNumber: driver.phoneNumber
        }
        
      }
    };
    return this.http.post<any>(this.apiUrl, payload); 
  }

  getDriver(driverId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${driverId}`);
  }

 updateDriver(driverId: number, driver: any): Observable<any> {

    const data = {
      DicOfDic: {
        DATA: {
          driverName: driver.driverName,
          phoneNumber: driver.phoneNumber
        }
      }
    };
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.http.put<any>(`${this.apiUrl}/${driverId}`, data, { ...headers }).pipe(
      catchError((error: HttpErrorResponse) => {
        console.error('Update driver error:', error);
        return throwError(() => new Error(`Error updating driver: ${error.message}`));
      })
    );
  }
  
  

}
