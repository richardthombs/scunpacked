import { Component, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import * as _ from "lodash";

import { environment } from "../../environments/environment";
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-ship',
  templateUrl: './ship.component.html',
  styleUrls: ['./ship.component.scss']
})
export class ShipComponent implements OnInit {

  ship: any = null;
  grouped: _.Dictionary<any[]> = null;
  types: string[] = null;

  private typeMap = {
    "Seat": { group: "Interior", class: "Seat" },
    "Room": { group: "Interior", class: "Room" },
    "Display": { group: "Interior", class: "Display" },
    "ManneuverThruster": { group: "Propulsion", class: "Thruster" },
    "FuelIntake": { group: "Fuel", class: "Intake" },
    "FuelTank": { group: "Fuel", class: "Hydrogen fuel tank" },
    "Cooler": { group: "Systems", class: "Cooler" },
    "Radar": { group: "Sensors", class: "Radar" },
    "QuantumDrive": { group: "Propulsion", class: "Quantum drive" },
    "Avionics": { group: "Systems", class: "Avionics" },
    "QuantumFuelTank": { group: "Fuel", class: "Quantum fuel tank" },
    "TurretBase.MannedTurret": { group: "Offence", class: "Manned turret" },
    "Shield": { group: "Defence", class: "Shield" },
    "MainThruster": { group: "Propulsion", class: "Main engine" },
    "MissileLauncher": { group: "Offence", class: "Missile rack" },
    "WeaponDefensive": { group: "Defence" },
    "WeaponDefensive.CountermeasureLauncher": { group: "Defence", class: "Countermeasure launcher" },
    "CoolerController": { group: "Controller" },
    "ShieldController": { group: "Controller" },
    "EnergyController": { group: "Controller" },
    "WeaponController": { group: "Controller" },
    "FlightController": { group: "Controller" },
    "CommsController": { group: "Controller" },
    "DoorController": { group: "Controller" },
    "LightController": { group: "Controller" },
    "Cargo": { group: "Cargo" },
    "SeatAccess": { group: "Interior" },
    "Door": { group: "Interior", class: "Door" },
    "Scanner": { group: "Sensors", class: "Scanner" },
    "Ping": { group: "Sensors", class: "Ping" },
    "Transponder": { group: "Systems", class: "Transponder" },
    "Turret": { group: "Offence" },
    "Turret.NoseMounted": { group: "Offence", class: "Turret" },
    "PowerPlant": { group: "Systems", class: "Power plant" },
    "Armor": { group: "Defence", class: "Armor" },
    "SelfDestruct": { group: "Systems", class: "Self destruct" },
    "SeatDashboard": { group: "Interior", class: "Dashboard" },
    "LandingSystem": { group: "Systems", class: "Landing system" }
  }

  constructor(private $http: HttpClient, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.$http.get<any[]>(`${environment.api}/ships/${params.get("name")}.json`).subscribe(r => {
        this.ship = r;

        this.grouped = _.groupBy(this.ship.Loadout, x => {
          if (!x.types) return "unknown";

          var grouping = { type: "unknown", group: "unknown" }
          Object.keys(x.types).forEach(type => {
            if (this.typeMap[type]) grouping = this.typeMap[type];
            else {
              var major = type.split(".")[0];
              if (this.typeMap[major]) grouping = this.typeMap[major];
            }
            if (grouping) {
              grouping.type = type;
              x.grouping = grouping;
            }
          });
          return grouping.group;
        });

        _.forEach(this.grouped, (value, key) => this.grouped[key] = _.orderBy(this.grouped[key], ["grouping.type", "maxsize", "port"], ["asc", "desc", "asc"]));

        console.log(this.grouped);

        this.types = [];
        this.ship.Loadout.forEach(x => {
          if (x.types) {
            Object.keys(x.types).forEach(t => {
              if (!this.types.includes(t)) this.types.push(t);
            })
          }
        });
      });
    });
  }

}
