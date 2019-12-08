import { Component, OnInit } from '@angular/core';
import { LocalisePipe } from './localise.pipe';
import { HttpClient } from '@angular/common/http';

import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor(private $http: HttpClient) { }

  async ngOnInit(): Promise<void> {
    var labels = await this.$http.get(`${environment.api}/labels.json`).toPromise();
    LocalisePipe.SetLabels(labels);
  }

}
