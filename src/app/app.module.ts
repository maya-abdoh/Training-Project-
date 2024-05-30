import { NgModule, LOCALE_ID } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatNativeDateModule, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AppComponent } from './app.component';
import { VehicleListComponent } from './components/vehicle-list/vehicle-list.component';
import { VehicleDetailComponent } from './components/vehicle-detail/vehicle-detail.component';
import { DriverListComponent } from './components/driver-list/driver-list.component';
import { AddDriverComponent } from './components/add-driver/add-driver.component';
import { AddVehicleComponent } from './components/add-vehicle/add-vehicle.component';
import { UpdateVehicleComponent } from './components/update-vehicle/update-vehicle.component';
import { AddRouteHistoryComponent } from './components/add-route-history/add-route-history.component';
import { RouteHistoryComponent } from './components/route-history/route-history.component';
import { VehicleService } from './vehicle.service';
import { DriverService } from './driver.service';
import { GeofenceService } from './geofence.Service';
import { registerLocaleData } from '@angular/common';
import localeEn from '@angular/common/locales/en';
import { WebSocketService } from './WebSocketService';

registerLocaleData(localeEn, 'en');

@NgModule({
  declarations: [
    AppComponent,
    VehicleListComponent,
    DriverListComponent,
    AddDriverComponent,
    AddVehicleComponent,
    UpdateVehicleComponent,
    AddRouteHistoryComponent,
    RouteHistoryComponent,
    VehicleDetailComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatInputModule,
    MatTableModule,
    MatCardModule,
    MatSelectModule,
    MatFormFieldModule,
    MatDividerModule,
    MatIconModule,
    MatMenuModule,
    MatNativeDateModule,
    MatDatepickerModule,  
    ReactiveFormsModule,
    FormsModule,
    MatSnackBar,
    RouterModule.forRoot([])
  ],
  providers: [
    VehicleService,
    DriverService,
    GeofenceService,
    WebSocketService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
