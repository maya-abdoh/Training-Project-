import { Component, OnInit } from '@angular/core';
import { GeofenceService } from '../../geofence.Service';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common'; 

@Component({
  selector: 'app-geofence',
  templateUrl: './geofence.component.html',
  styleUrls: ['./geofence.component.css'],
  standalone: true,
  imports: [
    CommonModule, 
    MatTableModule,
    MatCardModule
  ]
})
export class GeofenceComponent implements OnInit {
  geofences: any[] = [];

  displayedColumns: string[] = [
    'geofenceID',
    'geofenceType',
    'addedDate',
    'strokeColor',
    'strokeOpacity',
    'strokeWeight',
    'fillColor',
    'fillOpacity'
  ];

  constructor(private geofenceService: GeofenceService) { }

  ngOnInit(): void {
    this.geofenceService.getGeofences().subscribe(response => {
      if (response.sts === 1) {
        this.geofences = response.data.dicOfDT.Geofences;
      } else {
        console.error('Failed to fetch geofences', response);
      }
    }, error => {
      console.error('Geofence API call error:', error);
    });
  }
}
