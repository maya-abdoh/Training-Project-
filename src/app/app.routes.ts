import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VehicleListComponent } from './components/vehicle-list/vehicle-list.component';
import { VehicleDetailComponent } from './components/vehicle-detail/vehicle-detail.component';
import { DriverListComponent } from './components/driver-list/driver-list.component';
import { AddDriverComponent } from './components/add-driver/add-driver.component';
import { AddVehicleComponent } from './components/add-vehicle/add-vehicle.component';
import { UpdateVehicleComponent } from './components/update-vehicle/update-vehicle.component';
import { UpdateDriverComponent } from './components/update-driver/update-driver.component';
import { AddRouteHistoryComponent } from './components/add-route-history/add-route-history.component';
import { RouteHistoryComponent } from './components/route-history/route-history.component';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

const routes: Routes = [
  { path: 'vehicles', component: VehicleListComponent },
  { path: 'vehicles/:id', component: VehicleDetailComponent },
  { path: 'drivers', component: DriverListComponent },
  { path: 'add-driver', component: AddDriverComponent },
  { path: 'add-vehicle', component: AddVehicleComponent },
  { path: 'update-driver/:id', component: UpdateDriverComponent },
  { path: 'update-vehicle/:id', component: UpdateVehicleComponent },
  { path: 'add-route-history', component: AddRouteHistoryComponent },
  { path: 'route-history/:id', component: RouteHistoryComponent },
  { path: '', redirectTo: '/vehicles', pathMatch: 'full' }, 
  { path: '**', redirectTo: '/vehicles', pathMatch: 'full' } 
];


@NgModule({
  imports: [RouterModule.forRoot(routes) ,ToastrModule.forRoot(),    BrowserAnimationsModule, 
],
  exports: [RouterModule , ]
})
export class AppRoutingModule { }

export { routes };
