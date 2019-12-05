import { Component, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import * as _ from "lodash";

import { environment } from "../../environments/environment";

@Component({
  selector: 'app-shiplist',
  templateUrl: './shiplist.component.html',
  styleUrls: ['./shiplist.component.scss']
})
export class ShipListComponent implements OnInit {
  ships: any[] = [];

  constructor(private $http: HttpClient) { }

  ngOnInit() {
    this.$http.get<any[]>(`${environment.api}/ships.json`).subscribe(r => {
      this.ships = _.sortBy(r, "ClassName");
    });
  }

  apiUrl(ship: any) {
    return `${environment.api}/${ship.JsonFilename}`;
  }

}
