import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DashboardItem } from './dashboard-item';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  siteGroups: { [groupName: string]: DashboardItem[] } = {};
  siteGroupNames: string[] = [];


  constructor(private client: HttpClient) {

  }
  async ngOnInit(): Promise<void> {
    var sites = await this.client.get<DashboardItem[]>('/api/Endpoints').toPromise();
    this.siteGroupNames = sites.map(s => s.name.split('')[0]).filter((v,i,a) => a.indexOf(v) == i);
    this.siteGroups = {};
    this.siteGroupNames.forEach(n => {
      this.siteGroups[n] = sites.filter(s => s.name.startsWith(n)).map(s => ({ name: s.name.replace(n+ ' ',''), url: s.url }));
    })
  }
  title = 'ClientApp';
}
