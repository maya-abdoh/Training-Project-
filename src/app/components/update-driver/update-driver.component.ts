import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { DriverService } from '../../driver.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-update-driver',
  templateUrl: './update-driver.component.html',
  styleUrls: ['./update-driver.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule ,  
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule]
})
export class UpdateDriverComponent implements OnInit {
  updateDriverForm!: FormGroup;
  driverId!: string;

  constructor(
    private fb: FormBuilder,
    private driverService: DriverService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.driverId = this.route.snapshot.paramMap.get('id')??'-1';

    this.updateDriverForm = this.fb.group({
      driverName: ['', Validators.required],
      phoneNumber: ['', Validators.required]
    });

    this.route.params.subscribe(params => {
      this.driverService.getDriver(parseInt(this.driverId)).subscribe({
        next: (data) => {
          const driver = data.data.dicOfDic.DRIVER;
          console.log(driver)
          this.updateDriverForm.patchValue({
            driverName: driver.DriverName,
            phoneNumber: driver.Phonenumber
          });
        },
        error: (err) => {
          console.error('Error fetching driver details', err);
        }
      });
    });
  }

  onSubmit() {
    const driverData = {
      driverName: this.updateDriverForm.value.driverName,
      phoneNumber: this.updateDriverForm.value.phoneNumber ? parseInt(this.updateDriverForm.value.phoneNumber) : null
    };

    this.driverService.updateDriver(parseInt(this.driverId), driverData)
      .subscribe({
        next: (response) => {
          console.log('Driver updated successfully', response);
          this.router.navigate(['/drivers']); 
        },
        error: (error) => {
          console.error('Error updating driver', error);
          alert(`Error updating driver: ${error.message}`);
        }
      });
  }
}
