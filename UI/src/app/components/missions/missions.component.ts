import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { MissionService } from '../../services/mission.service';

@Component({
  selector: 'app-mission',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: 'missions.component.html'
})
export class MissionComponent implements OnInit {
  missionType = new FormControl('latest');
  missionData: any = null;

  constructor(private missionService: MissionService, private cd: ChangeDetectorRef, private router: Router) {}

  ngOnInit(): void {
    this.loadMissions();
    this.missionType.valueChanges.subscribe(() => this.loadMissions());
  }

  loadMissions() {
    const type = this.missionType.value;
    this.missionData = null;

    if (type === 'latest') {
      this.missionService.getLatest().subscribe(data => {
        this.missionData = data;
        this.cd.detectChanges();
        });
    } else if (type === 'upcoming') {
      this.missionService.getUpcoming().subscribe(data => {
        this.missionData = data;
        this.cd.detectChanges();
        });
    } else if (type === 'past') {
      this.missionService.getPast().subscribe(data => {
        this.missionData = data;
        this.cd.detectChanges();
        });
    }
  }

  logout(){
    localStorage.removeItem('token'); // optional: clear invalid token
    this.router.navigate(['/login']); // redirect to login
  }
}