import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { VehicleService } from '../../vehicle.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-update-vehicle',
  templateUrl: './update-vehicle.component.html',
  styleUrls: ['./update-vehicle.component.css'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ]
})
export class UpdateVehicleComponent implements OnInit {
  vehicleForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private vehicleService: VehicleService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {

    this.vehicleForm = this.fb.group({
      VehicleNumber: ['', Validators.required],
      DriverId:['',Validators.required],
      VehicleType: ['', Validators.required],
      VehicleModel: ['', Validators.required],
      VehicleMake: ['', Validators.required],
      PurchaseDate: ['', Validators.required]
    });

    const vehicleId = this.route.snapshot.paramMap.get('id');
    if (vehicleId) {
      this.vehicleService.getVehicleDetails(parseInt(vehicleId)).subscribe({
        next: (response) => {

    
          if (response ) {
            const vehicle = response.data.dicOfDic.Vehicle;
            const dateParts = vehicle.PurchaseDate.split('-'); 
            const dateObject = new Date(parseInt(dateParts[2]), parseInt(dateParts[1]) - 1, parseInt(dateParts[0])); 

            this.vehicleForm.patchValue({
              VehicleNumber: vehicle.VehicleNumber,
              VehicleType: vehicle.VehicleType,
              DriverId:vehicle.DriverId,
              VehicleModel: vehicle.VehicleModel,
              VehicleMake: vehicle.VehicleMake,
              PurchaseDate: dateObject.getTime(),

            });
          }
        },
        error: (error) => console.error('Failed to fetch vehicle details', error)
      });
    }
  }

  onSubmit(): void {
    if (this.vehicleForm.valid) {
      const vehicleId = this.route.snapshot.paramMap.get('id');
      if (vehicleId) {
        const payload = {
          dicOfDic: {
            DATA: {
              ...this.vehicleForm.value
            }
          }
        };

        console.log('Sending data:', payload);

        this.vehicleService.updateVehicle(parseInt(vehicleId), payload).subscribe({
          next: (response) => {
            console.log('Vehicle updated successfully', response);
            this.router.navigate(['/vehicles']);
          },
          error: (error) => {
            console.error('Failed to update vehicle', error);
          }
        });
      }
    }
  }
}
