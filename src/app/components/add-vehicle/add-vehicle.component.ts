import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { VehicleService } from '../../vehicle.service';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-add-vehicle',
  templateUrl: './add-vehicle.component.html',
  styleUrls: ['./add-vehicle.component.css'],
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule
  ]
})
export class AddVehicleComponent implements OnInit {
  vehicleForm!: FormGroup;

  constructor(private fb: FormBuilder, private vehicleService: VehicleService) { }

  ngOnInit(): void {
    this.vehicleForm = this.fb.group({
      DriverId: ['', Validators.required],
      VehicleNumber: ['', Validators.required],
      VehicleType: ['', Validators.required],
      VehicleMake: ['', Validators.required],
      VehicleModel: ['', Validators.required],
      PurchaseDate: ['', Validators.required]
    });
  }
  
  addVehicle() {
    if (this.vehicleForm.valid) {
      this.vehicleService.addVehicle(this.vehicleForm.value).subscribe(
        response => {
          console.log('Vehicle added successfully:', response);
          alert('Vehicle added successfully.');
        },
        error => {
          console.error('Error adding vehicle:', error);
          alert(`Error adding vehicle: ${error.error?.Error || error.message}`);
        }
      );
    }
  }
}
