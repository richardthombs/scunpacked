import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as _ from 'lodash';

import { ShipService } from '../ship.service';
import { Ship } from '../Ship';


@Component({
  selector: 'app-compare',
  templateUrl: './compare-ships-page.component.html',
  styleUrls: ['./compare-ships-page.component.scss']
})
export class CompareShipsPage implements OnInit {

  ships: Ship[] = [];

  private currentSortField: string = "vehicleName";
  private currentSortDirection: "asc" | "desc" = "asc";

  constructor(private shipSvc: ShipService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.queryParamMap.subscribe(params => {

      let shipsParam: string = params.get("ships") || "";
      let shipClasses = shipsParam.split(",");

      let shipPromises: Promise<Ship>[] = [];

      this.ships = [];
      for (let i = 0; i < shipClasses.length; i++) {
        shipPromises[i] = this.shipSvc.load(shipClasses[i]).then(s => { this.ships.push(s); this.applySort(); return s; });
      }

      Promise.all(shipPromises).then((ships: Ship[]) => {
        //this.sortBy(this.currentSortField);
      });

    });
  }

  sortBy(field: string, defaultDirection?: "asc" | "desc") {
    if (field == this.currentSortField) this.currentSortDirection = this.currentSortDirection == "asc" ? "desc" : "asc";
    else this.currentSortDirection = defaultDirection || "asc";
    this.currentSortField = field;
    this.applySort();
  }

  applySort() {
    this.ships = _.orderBy(this.ships, [s => _.get(s, this.currentSortField)], [this.currentSortDirection]);
  }

}
