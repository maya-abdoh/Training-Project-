import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { DriverService } from '../../driver.service';
import { Router } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  standalone: true,
  selector: 'app-add-driver',
  templateUrl: './add-driver.component.html',
  styleUrls: ['./add-driver.component.css'],
  imports: [
    ReactiveFormsModule, 
    MatFormFieldModule, 
    MatInputModule, 
    MatButtonModule,
    FormsModule,

    
  ], 
})
export class AddDriverComponent implements OnInit {
  addDriverForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private driverService: DriverService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.addDriverForm = this.fb.group({
      driverName: ['', Validators.required],
      phoneNumber: ['', Validators.required],
    });
  }

  onSubmit(): void {
    if (this.addDriverForm.valid) {
      console.log('Submitting:', this.addDriverForm.value);
      this.driverService.addDriver(this.addDriverForm.value).subscribe({
        next: (response) => {
          console.log('Driver added successfully', response);
          alert('Driver added successfully');
          this.addDriverForm.reset();
        },
        error: (err) => {
          console.error('Error adding driver', err);
          alert('Error adding driver');
        }
      });
    }
  }
}
