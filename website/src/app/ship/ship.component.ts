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
  grouped: any = null;
  types: string[] = null;

  private typeMap = {
    "Seat": "Interior",
    "Room": "Interior",
    "Display": "Interior",
    "ManneuverThruster": "Propulsion",
    "FuelIntake": "Fuel",
    "FuelTank": "Fuel",
    "Cooler": "Systems",
    "Radar": "Sensors",
    "QuantumDrive": "Propulsion",
    "Avionics": "Systems",
    "QuantumFuelTank": "Fuel",
    "TurretBase": "Offence",
    "Shield": "Defence",
    "MainThruster": "Propulsion",
    "MissileLauncher": "Offence",
    "WeaponDefensive": "Defence",
    "CoolerController": "Interior",
    "ShieldController": "Interior",
    "Cargo": "Cargo",
    "SeatAccess": "Interior",
    "Door": "Interior",
    "Scanner": "Sensors",
    "Ping": "Sensors",
    "Transponder": "Systems",
    "Turret": "Offence",
    "PowerPlant": "Systems",
    "Armor": "Defence",
    "EnergyController": "Interior",
    "WeaponController": "Interior",
    "SelfDestruct": "Systems",
    "SeatDashboard": "Interior",
    "FlightController": "Interior",
    "CommsController": "Interior",
    "DoorController": "Interior",
    "LightController": "Interior",
    "LandingSystem": "Systems"
  }

  constructor(private $http: HttpClient, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.$http.get<any[]>(`${environment.api}/ships/${params.get("name")}.json`).subscribe(r => {
        this.ship = r;

        this.grouped = _.groupBy(this.ship.Loadout, x => {
          if (!x.types) return "unknown";

          var grouping = "unknown"
          Object.keys(x.types).forEach(type => {
            if (this.typeMap[type]) grouping = this.typeMap[type];
          });
          return grouping;
        });
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
