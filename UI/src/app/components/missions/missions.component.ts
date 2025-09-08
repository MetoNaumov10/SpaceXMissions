import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
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

  constructor(private missionService: MissionService) {}

  ngOnInit(): void {
    this.loadMissions();
    this.missionType.valueChanges.subscribe(() => this.loadMissions());
  }

  loadMissions() {
    const type = this.missionType.value;

    if (type === 'latest') {
      this.missionService.getLatest().subscribe(d => this.missionData = d);
    } else if (type === 'upcoming') {
      this.missionService.getUpcoming().subscribe(d => this.missionData = d);
    } else if (type === 'past') {
      this.missionService.getPast().subscribe(d => this.missionData = d);
    }
  }
}