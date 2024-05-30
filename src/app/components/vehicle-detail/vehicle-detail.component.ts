import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VehicleService } from '../../vehicle.service';
import { DriverService } from '../../driver.service'; 
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule, DatePipe } from '@angular/common'; 

@Component({
  selector: 'app-vehicle-detail',
  templateUrl: './vehicle-detail.component.html',
  styleUrls: ['./vehicle-detail.component.css'],
  standalone: true,
  imports: [
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatDividerModule,
    MatIconModule,
    MatSelectModule,
    CommonModule
  ],
  providers: [DatePipe]
})
export class VehicleDetailComponent implements OnInit {
  vehicle: any = null;
  drivers: any[] = [];
  selectedDriverId: number | null = null;
  error: string = '';

  constructor(
    private vehicleService: VehicleService,
    private driverService: DriverService, 
    private route: ActivatedRoute,
    private router: Router,
    private datePipe: DatePipe 
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const vehicleId = +params['id'];
      this.loadVehicleDetails(vehicleId);
    });
    this.loadDrivers();
  }

  loadVehicleDetails(vehicleId: number): void {
    this.vehicleService.getVehicleDetails(vehicleId).subscribe({
      next: (response) => {
        if (response) {
          this.vehicle = response.data.dicOfDic.Vehicle;
          this.selectedDriverId = this.vehicle?.DriverId || null;
          console.log("Loaded vehicle:", this.vehicle);
        } else {
          this.error = 'No vehicle information found';
        }
      },
      error: (error) => {
        this.error = 'Failed to load vehicle details';
        console.error('Error loading vehicle details:', error);
      }
    });
  }

  loadDrivers(): void {
    this.driverService.getDrivers().subscribe({
      next: (response) => {
        if (response && response.data && Array.isArray(response.data.dicOfDT.Drivers)) {
          this.drivers = response.data.dicOfDT.Drivers;
          console.log("Loaded drivers:", this.drivers); 
        } else {
          console.error("Response does not contain drivers array", response);
        }
      },
      error: (error) => {
        this.error = 'Failed to load drivers';
        console.error('Error loading drivers:', error);
      }
    });
  }
  
  getFormattedDate(epoch: number): string {
    return this.datePipe.transform(epoch * 1000, 'yyyy-MM-dd HH:mm:ss') || '';
  }
}
