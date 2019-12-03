import { Component, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";

import { environment } from "../environments/environment";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'scdb';
  ships: any[] = [];

  constructor(private $http: HttpClient) {

  }

  ngOnInit() {
    this.$http.get<any[]>(`${environment.api}/ships.json`).subscribe(r => {
      this.ships = r;
    });
  }

  apiUrl(ship) {
    return `${environment.api}/${ship.json}`;
  }
}
