import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  private vehiclesUrl = 'https://localhost:7276/api/Vehicles';

  constructor(private http: HttpClient) { }

  getVehicles(): Observable<any> {
    return this.http.get(this.vehiclesUrl).pipe(
      catchError(this.handleError)
    );
  }

getVehicleDetails(vehicleId: number): Observable<any> {
  return this.http.get(`${this.vehiclesUrl}/${vehicleId}`);
}
addVehicle(vehicle: any): Observable<any> {
  const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  const gvarPayload = {
    DicOfDic: {
      DATA: {
        driverId: vehicle.DriverId.toString(),
        VehicleNumber: vehicle.VehicleNumber.toString(),
        VehicleType: vehicle.VehicleType,
        VehicleMake: vehicle.VehicleMake,
        VehicleModel: vehicle.VehicleModel,
        PurchaseDate: (new Date(vehicle.PurchaseDate)).getTime().toString()
      }
    }
  };
  return this.http.post<any>(this.vehiclesUrl, gvarPayload, { headers }).pipe(
    tap(response => console.log('Response:', response)),
    catchError(this.handleError)
  );
}

  private handleError(error: HttpErrorResponse) {
    console.error('Error adding vehicle:', error.message, error.status, error.url);
    return throwError('Error adding vehicle. Please try again later.');
  }


  updateVehicle(vehicleId: number, payload: any): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
  
  
    return this.http.put(`${this.vehiclesUrl}/${vehicleId}`, payload, { headers }).pipe(
      catchError((error: any) => {
        console.error('Update vehicle error:', error);
        return throwError(() => error);
      })
    );
  }
  
  deleteVehicle(vehicleId: number): Observable<any> {
    return this.http.delete<any>(`${this.vehiclesUrl}/${vehicleId}`).pipe(
      catchError((error: any) => {
        console.error('Delete vehicle error:', error);
        return throwError(() => error);
      })
    );
  }
}
