import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { VehicleService } from '../../vehicle.service';
import { RouteHistoryService } from '../../route-history.service';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-add-route-history',
  templateUrl: './add-route-history.component.html',
  styleUrls: ['./add-route-history.component.css'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatOptionModule,
    MatCardModule
  ]
})
export class AddRouteHistoryComponent implements OnInit {
  routeHistoryForm!: FormGroup;
  vehicles: any[] = [];

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private vehicleService: VehicleService,
    private routeHistoryService: RouteHistoryService
  ) {}

  ngOnInit(): void {
    this.routeHistoryForm = this.fb.group({
      vehicleID: ['', Validators.required],
      gpsTime: ['', Validators.required],
      address: ['', Validators.required],
      latitude: ['', Validators.required],
      longitude: ['', Validators.required],
      vehicleDirection: ['', Validators.required],
      status: ['', Validators.required],
      gpsSpeed: ['', Validators.required]
    });

    this.loadVehicles();
  }

  loadVehicles(): void {
    this.vehicleService.getVehicles().subscribe({
      next: (response) => {
        console.log('Vehicle API response:', response);
        if (response.dicOfDT && response.dicOfDT.Vehicles) {
          this.vehicles = response.dicOfDT.Vehicles;
          console.log('Vehicles:', this.vehicles);
        } else {
          console.error('Failed to fetch vehicles:', response);
        }
      },
      error: (err) => {
        console.error('Error fetching vehicles:', err);
      }
    });
  }

  onSubmit(): void {
    if (this.routeHistoryForm.valid) {
      const routeHistoryData = this.routeHistoryForm.value;
      this.routeHistoryService.addRouteHistory(routeHistoryData).subscribe({
        next: () => {
          alert('Route history added successfully');
          this.router.navigate(['/route-history']); 
        },
        error: (err) => {
          console.error('Error adding route history', err);
          alert('Failed to add route history. Please check the console for more details.');
        }
      });
    }
  }
}
