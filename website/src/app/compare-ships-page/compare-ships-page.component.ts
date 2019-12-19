import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as _ from 'lodash';

import { ShipService } from '../ship.service';
import { Ship } from '../Ship';
import { ComparisonGroup, ComparisonField } from '../Comparisons';
import { LocalisationService } from '../localisation.service';


@Component({
  selector: 'app-compare',
  templateUrl: './compare-ships-page.component.html',
  styleUrls: ['./compare-ships-page.component.scss']
})
export class CompareShipsPage implements OnInit {

  fields: ComparisonGroup<Ship>[] = [
    new ComparisonGroup({
      title: "",
      fields: [
        new ComparisonField({ title: "Name", valueFn: s => this.localisationSvc.getText(s.vehicleName, s.className), linkFn: s => `/ships/${s.className.toLowerCase()}` }),
        new ComparisonField({ title: "Size", valueFn: s => s.vehicleSize, formatFn: v => `Size ${v}`, sortDirection: "desc", compareFn: undefined }),
        new ComparisonField({ title: "Career", valueFn: s => this.localisationSvc.getText(s.vehicleCareer), compareFn: undefined }),
        new ComparisonField({ title: "Role", valueFn: s => this.localisationSvc.getText(s.vehicleRole), compareFn: undefined }),
      ]
    }),
    new ComparisonGroup({
      title: "Capacity",
      fields: [
        new ComparisonField({ title: "Crew", valueFn: s => s.crewSize, units: "crew" }),
        new ComparisonField({ title: "Cargo", valueFn: s => s.scu, units: "SCU", sortDirection: "desc" }),
      ],
    }),
    new ComparisonGroup({
      title: "Durability",
      fields: [
        new ComparisonField({ title: "Total HP", units: "HP", valueFn: s => s.hitPoints, sortDirection: "desc" }),
      ]
    }),
    new ComparisonGroup({
      title: "Shields",
      fields: [
        new ComparisonField({ title: "Shield HP", units: "HP", valueFn: s => s.maxShieldHealth, sortDirection: "desc" }),
      ]
    }),
    new ComparisonGroup({
      title: "Manuverability",
      fields: [
        new ComparisonField({ title: "Max speed", units: "m/s", valueFn: s => s.maxSpeed, sortDirection: "desc" }),
        new ComparisonField({ title: "SCM speed", units: "m/s", valueFn: s => s.scmSpeed, sortDirection: "desc" }),
      ]
    }),
    new ComparisonGroup({
      title: "Quantum travel",
      fields: [
        new ComparisonField({ title: "Quantum range", units: "m", siPrefix: true, valueFn: s => s.quantumRange, sortDirection: "desc" }),
        new ComparisonField({ title: "Quantum speed", units: "m/s", siPrefix: true, valueFn: s => s.quantumSpeed, sortDirection: "desc" }),
        new ComparisonField({ title: "Quantum fuel tank", units: "l", valueFn: s => s.quantumFuelCapacity, sortDirection: "desc" }),
        new ComparisonField({ title: "PO to ArcCorp (time)", units: "s", valueFn: s => s.secondsToArcCorp }),
        new ComparisonField({ title: "PO to ArcCorp (fuel)", units: "l", valueFn: s => s.fuelToArcCorp }),
        new ComparisonField({ title: "PO to ArcCorp and back", decimals: 1, units: "times", valueFn: s => s.timesToArcCorpAndBack, sortDirection: "desc" }),
      ]
    })

  ]

  ships: Ship[] = [];

  private currentSortField: ComparisonField<Ship> = this.fields[0].fields[0];
  private currentSortDirection: "asc" | "desc" = "asc";

  constructor(private shipSvc: ShipService, private route: ActivatedRoute, private localisationSvc: LocalisationService) { }

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
        this.fields.forEach(g => {
          g.fields.forEach(f => {
            if (g.visibleFn && g.visibleFn(this.ships)) this.ships.forEach(i => {
              let value = f.valueFn(i);
              if (value === undefined || typeof value !== "number") return;
              if (f.sortDirection == "asc" && (f.bestValue === undefined || value < f.bestValue)) f.bestValue = value;
              if (f.sortDirection == "asc" && (f.worstValue === undefined || value > f.worstValue)) f.worstValue = value;
              if (f.sortDirection == "desc" && (f.bestValue === undefined || value > f.bestValue)) f.bestValue = value;
              if (f.sortDirection == "desc" && (f.worstValue === undefined || value < f.worstValue)) f.worstValue = value;
            });
          });
        });
      });

    });
  }

  toggleCollapse(group: ComparisonGroup<Ship>) {
    group.collapsed = !group.collapsed;
  }

  sortBy(field: ComparisonField<Ship>) {
    if (field === this.currentSortField) this.currentSortDirection = this.currentSortDirection == "asc" ? "desc" : "asc";
    else this.currentSortDirection = field.sortDirection;
    this.currentSortField = field;
    this.applySort();
  }

  private applySort() {
    this.ships = _.orderBy(this.ships, [i => this.currentSortField.valueFn(i) || 0], [this.currentSortDirection]);
  }

}
