import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DashboardItem } from './dashboard-item';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  sites!: DashboardItem[];

  constructor(private client: HttpClient) {

  }
  async ngOnInit(): Promise<void> {
    this.sites = await this.client.get<DashboardItem[]>('/api/Endpoints').toPromise();
  }
  title = 'ClientApp';
}

