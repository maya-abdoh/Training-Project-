import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RouteHistoryService } from '../../route-history.service';
import { DatePipe, CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-route-history',
  templateUrl: './route-history.component.html',
  styleUrls: ['./route-history.component.css'],
  standalone: true,
  imports: [
    CommonModule, FormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatTableModule, MatCardModule
  ],
  providers: [DatePipe]
})
export class RouteHistoryComponent implements OnInit {
  routeHistories: any[] = [];
  vehicleId!: number;
  displayedColumns: string[] = ['epoch', 'address', 'latitude', 'longitude', 'position', 'vehiclespeed', 'status', 'vehicledirection'];

  constructor(
    private route: ActivatedRoute,
    private routeHistoryService: RouteHistoryService,
    private datePipe: DatePipe
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const vehicleIdStr = params.get('id');
      if (vehicleIdStr) {
        this.vehicleId = +vehicleIdStr;
        this.fetchRouteHistory(); 
      }
    });
  }

  fetchRouteHistory(): void {
    this.routeHistoryService.getRouteHistory(this.vehicleId)
      .subscribe({
        next: (response) => {
          if (response.sts === 1) {
            this.routeHistories = response.data.dicOfDT.RouteHistory;
          } else {
            console.error('RouteHistory data is missing or incorrect:', response);
          }
        },
        error: (err) => console.error('Error fetching route history:', err)
      });
  }
}
