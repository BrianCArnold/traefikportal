import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DashboardItem } from './dashboard-item';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  siteGroups: { [groupName: string]: { typeColor: string, textColor: string, items: DashboardItem[] } }= {};
  siteGroupNames: string[] = [];


  constructor(private client: HttpClient) {

  }
  async ngOnInit(): Promise<void> {
    setInterval(() => this.populateSites().then(), 5000);
  }
  async populateSites(): Promise<void> {
    var sites: DashboardItem[] = await this.client.get<DashboardItem[]>('/api/Endpoints').toPromise();
    this.siteGroupNames = sites.map(s => s.name.split(' ')[0].trim()).filter((v,i,a) => a.indexOf(v) == i);
    this.siteGroups = {};
    var siteBackgrounds = ['primary', 'success', 'danger', 'secondary'];
    this.siteGroupNames.forEach((n,i) => {
      this.siteGroups[n] = { typeColor: siteBackgrounds[i], textColor: 'white', items: sites.filter(s => s.name.startsWith(n)).map(s => ({ name: s.name.replace(n,'').trim(), url: s.url })) };
    });
  }
  title = 'ClientApp';
}
