import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../../vehicle.service';
import { GeofenceService } from '../../geofence.Service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatMenuModule } from '@angular/material/menu';
import { CommonModule } from '@angular/common';
import { GeofenceComponent } from '../geofence/geofence.component';
import { WebSocketService } from '../../WebSocketService';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    MatDividerModule,
    MatIconModule,
    MatTableModule,
    MatMenuModule,
    GeofenceComponent
  ]
})
export class VehicleListComponent implements OnInit {
  vehicles: any[] = [];
  geofences: any[] = [];

  displayedColumns: string[] = [
    'vehicleID',
    'vehicleNumber',
    'vehicleType',
    'lastDirection',
    'lastStatus',
    'lastAddress',
    'lastLatitude',
    'lastLongitude',
    'actions'
  ];

  constructor(
    private vehicleService: VehicleService,
    private geofenceService: GeofenceService,
    private router: Router,  
    private webSocketService: WebSocketService,
    private snackBar: MatSnackBar 

  ) { }

  ngOnInit(): void {
    this.loadVehicles();
    this.loadGeofences();

    if (typeof window !== 'undefined') {
      this.webSocketService.onMessage().subscribe((message: string) => {
        console.log('WebSocket message received:', message);
        this.handleWebSocketMessage(message);
      });

      this.webSocketService.waitForConnection().then(() => {
        this.webSocketService.sendMessage('Hello from client');
      }).catch(error => {
        console.error('Failed to send initial message:', error);
      });
    }
  }

loadVehicles(): void {
  this.vehicleService.getVehicles().subscribe(
    response => {
      console.log('API response:', response);
      if (response && response.dicOfDT && response.dicOfDT.Vehicles) {
        this.vehicles = response.dicOfDT.Vehicles;
        console.log('Processed vehicles:', this.vehicles);
      } else {
        console.error('Failed to fetch vehicles', response);
        this.snackBar.open('Failed to fetch vehicles', 'Close', { duration: 3000 });
      }
    },
    error => {
      console.error('Vehicle API call error:', error);
      this.snackBar.open('Error fetching vehicles: ' + error.message, 'Close', { duration: 3000 });
    }
  );
}

loadGeofences(): void {
  this.geofenceService.getGeofences().subscribe(
    response => {
      console.log('Geofence API response:', response);
      if (response && response.data && response.data.dicOfDT && response.data.dicOfDT.Geofences) {
        this.geofences = response.data.dicOfDT.Geofences;
      } else {
        console.error('Failed to fetch geofences', response);
        this.snackBar.open('Failed to fetch geofences', 'Close', { duration: 3000 });
      }
    },
    error => {
      console.error('Geofence API call error:', error);
      this.snackBar.open('Error fetching geofences: ' + error.message, 'Close', { duration: 3000 });
    }
  );
  }
  

  deleteVehicle(vehicleId: number): void {
    if (confirm('Are you sure you want to delete this vehicle?')) {
      this.vehicleService.deleteVehicle(vehicleId).subscribe(response => {
        this.vehicles = this.vehicles.filter(vehicle => vehicle.vehicleid !== vehicleId);
      }, error => {
        console.error('Error deleting vehicle:', error);
      });
    }
  }

  openVehicleDetails(vehicleId: number): void {
    this.router.navigate(['/vehicles', vehicleId]);
  }

  openUpdateVehiclePage(vehicleId: number): void {
    this.router.navigate(['/update-vehicle', vehicleId]);
  }

  getRoutes(vehicleId: number): void {
    this.router.navigate(['/route-history', vehicleId]);
  }

  handleWebSocketMessage(message: string): void {
    console.log('Received WebSocket message:', message);
    try {
      const data = JSON.parse(message);
      console.log('Parsed WebSocket data:', data);

      if (data.message) {
        console.log('Received message:', data.message);
        return;
      }

      if (data.$id && data.Action === 'Add' && data.Data) {
        const vehicleInfo = data.Data;

        const vehicleIndex = this.vehicles.findIndex(v => v.vehicleid === vehicleInfo.Vehicleid);

        const newVehicle = {
          vehicleid: vehicleInfo.Vehicleid,
          vehiclenumber: vehicleInfo.Vehiclenumber,
          vehicletype: vehicleInfo.Vehicletype,
          lastRouteHistory: {
            vehicledirection: vehicleInfo.VehicleInformation?.lastDirection || 'N/A',
            status: vehicleInfo.VehicleInformation?.lastStatus || 'N/A',
            address: vehicleInfo.VehicleInformation?.lastAddress || 'N/A',
            latitude: vehicleInfo.VehicleInformation?.lastLatitude || 'N/A',
            longitude: vehicleInfo.VehicleInformation?.lastLongitude || 'N/A'
          }
        };

        if (vehicleIndex !== -1) {
          this.vehicles[vehicleIndex] = newVehicle;
        } else {
          this.vehicles.push(newVehicle);
        }
        this.vehicles = [...this.vehicles]; 
        console.log('Updated vehicles:', this.vehicles);
      } else {
        console.warn('Unexpected WebSocket data format:', data);
      }
    } catch (error) {
      console.error('Failed to handle WebSocket message:', error);
    }
  }
}
