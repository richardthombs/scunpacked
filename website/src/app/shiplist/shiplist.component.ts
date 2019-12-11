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
  ships: ShipIndexEntry[] = [];
  grouped: { [id: string]: ShipIndexEntry[] } = {}
  doubleGrouped: { [id: string]: { [id: string]: ShipIndexEntry[] } } = {}

  constructor(private $http: HttpClient) { }

  ngOnInit() {
    this.$http.get<ShipIndexEntry[]>(`${environment.api}/ships.json`).subscribe(r => {
      r = r.filter(x => x.career != "@LOC_PLACEHOLDER");

      this.grouped = _.groupBy(r, x => x.career);
      _.each(this.grouped, (g, i) => this.grouped[i] = _.sortBy(g, s => s.className));

      console.log(this.grouped);

      _.each(this.grouped, (value, key) => this.doubleGrouped[key] = _.groupBy(this.grouped[key], x => x.role));

    });
  }
}

interface ShipIndexEntry {
  jsonFilename: string;
  name: string;
  career: string;
  role: string;
  className: string;
  type: string;
  subType: string;
}
