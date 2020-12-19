import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as _ from 'lodash';

import { ShipService } from '../ship.service';
import { Ship } from '../Ship';
import { ComparisonGroup, ComparisonField } from '../Comparisons';
import { LocalisationService } from '../Localisation';


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
        new ComparisonField({ title: "Name", valueFn: s => s.Name, linkFn: s => `/ships/${s.ClassName.toLowerCase()}` }),
        new ComparisonField({ title: "Size", valueFn: s => s.Size, formatFn: v => `Size ${v}`, sortDirection: "desc", compareFn: undefined }),
        new ComparisonField({ title: "Career", valueFn: s => s.Career, compareFn: undefined }),
        new ComparisonField({ title: "Role", valueFn: s => s.Role, compareFn: undefined }),
      ]
    }),
    new ComparisonGroup({
      title: "Capacity",
      fields: [
        new ComparisonField({ title: "CIG crew", valueFn: s => s.Crew }),
        new ComparisonField({ title: "Weapon crew", valueFn: s => s.WeaponCrew }),
        new ComparisonField({ title: "Operations crew", valueFn: s => s.OperationsCrew }),
        new ComparisonField({ title: "Cargo", valueFn: s => s.Cargo, units: "SCU", sortDirection: "desc" }),
      ],
    }),
    /*
    new ComparisonGroup({
      title: "Durability",
      fields: [
        new ComparisonField({ title: "Total HP", units: "HP", valueFn: s => s.hitPoints, sortDirection: "desc" }),
      ]
    }),*/
    /*
    new ComparisonGroup({
      title: "Shields",
      fields: [
        new ComparisonField({ title: "Shield HP", units: "HP", valueFn: s => s..maxShieldHealth, sortDirection: "desc" }),
        new ComparisonField({ title: "Regeneration", units: "HP/s", valueFn: s => s.maxShieldRegen, sortDirection: "desc" }),
      ]
    }),*/
    new ComparisonGroup({
      title: "Manuverability",
      fields: [
        new ComparisonField({ title: "Max speed", units: "m/s", valueFn: s => s.FlightCharacteristics.MaxSpeed, sortDirection: "desc" }),
        new ComparisonField({ title: "SCM speed", units: "m/s", valueFn: s => s.FlightCharacteristics.ScmSpeed, sortDirection: "desc" }),
      ]
    }),
    new ComparisonGroup({
      title: "Fuel usage",
      fields: [
        new ComparisonField({ title: "Hydrogen tank", units: "l", valueFn: s => s.Propulsion.FuelCapacity, sortDirection: "desc" }),
        new ComparisonField({ title: "Main thrusters", units: "l/s", valueFn: s => s.Propulsion.FuelUsage.Main }),
        new ComparisonField({ title: "Retro thrusters", units: "l/s", valueFn: s => s.Propulsion.FuelUsage.Retro }),
        new ComparisonField({ title: "Vtol thrusters", units: "l/s", valueFn: s => s.Propulsion.FuelUsage.Vtol }),
        new ComparisonField({ title: "Manuvering thrusters", units: "l/s", valueFn: s => s.Propulsion.FuelUsage.Maneuvering }),
        new ComparisonField({ title: "Fuel intakes", units: "l/s", valueFn: s => s.Propulsion.FuelIntakeRate, sortDirection: "desc" }),
        new ComparisonField({ title: "Time to empty", units: "s", valueFn: s => s.Propulsion.ManeuveringTimeTillEmpty, sortDirection: "desc" }),
      ]
    }),
    new ComparisonGroup({
      title: "Quantum travel",
      fields: [
        new ComparisonField({ title: "Quantum range", units: "m", siPrefix: true, valueFn: s => s.QuantumTravel.Range, sortDirection: "desc" }),
        new ComparisonField({ title: "Quantum speed", units: "m/s", siPrefix: true, valueFn: s => s.QuantumTravel.Speed, sortDirection: "desc" }),
        new ComparisonField({ title: "Quantum fuel tank", units: "l", valueFn: s => s.QuantumTravel.FuelCapacity, sortDirection: "desc" }),
        new ComparisonField({ title: "PO to ArcCorp (time)", units: "s", valueFn: s => s.QuantumTravel.PortOlisarToArcCorpTime }),
        new ComparisonField({ title: "PO to ArcCorp (fuel)", units: "l", valueFn: s => s.QuantumTravel.PortOlisarToArcCorpFuel }),
        new ComparisonField({ title: "PO to ArcCorp and back", decimals: 1, units: "times", valueFn: s => s.QuantumTravel.PortOlisarToArcCorpAndBack, sortDirection: "desc" }),
      ]
    }),
    new ComparisonGroup({
      title: "Insurance",
      fields: [
        new ComparisonField({ title: "Claim time", units: "minutes", valueFn: s => s.Insurance?.StandardClaimTime }),
        new ComparisonField({ title: "Expedited time", units: "minutes", valueFn: s => s.Insurance?.ExpeditedClaimTime }),
        new ComparisonField({ title: "Expedited cost", units: "aUEC", valueFn: s => s.Insurance?.ExpeditedCost }),
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
        shipPromises[i] = this.shipSvc.load(shipClasses[i]).then(s => { this.ships.push(s.Ship); this.applySort(); return s.Ship; });
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
