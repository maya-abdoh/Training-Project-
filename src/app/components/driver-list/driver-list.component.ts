import { Component, OnInit, Renderer2, ElementRef } from '@angular/core';
import { DriverService } from '../../driver.service';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import {MatTableModule} from '@angular/material/table';

@Component({
  selector: 'app-driver-list',
  templateUrl: './driver-list.component.html',
  styleUrls: ['./driver-list.component.css'],
  standalone: true,
  imports: [
    MatCardModule,
    MatButtonModule,
    MatTableModule,
  ]

})
  
export class DriverListComponent implements OnInit {
  displayedColumns: string[] = ['driverid', 'drivername', 'phonenumber', 'edit'];

  drivers : [] = []

  constructor(private driverService: DriverService, private router: Router) { }

  ngOnInit(): void {
    this.driverService.getDrivers().subscribe({
      next: (response) => {
        if (response.sts === 1) {
          this.drivers= response.data.dicOfDT.Drivers;
          console.log(this.drivers)
        } else {
          console.error('Invalid data structure:', response);
        }
      },
      error: (err) => {
        console.error('Error fetching drivers:', err);
      }
    });
  }
  public editDriver(driverid:number){
    this.router.navigate(['/update-driver', driverid]).catch(err => console.error('Navigation error:', err));
  }

}
