import { Component, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import * as _ from "lodash";

import { environment } from "../../environments/environment";
import { ActivatedRoute } from '@angular/router';
import { Z_UNKNOWN } from 'zlib';

@Component({
  selector: 'app-ship',
  templateUrl: './ship.component.html',
  styleUrls: ['./ship.component.scss']
})
export class ShipComponent implements OnInit {

  ship: Ship | null = null;
  grouped: any;
  types: string[] = [];

  private typeMap: { [id: string]: ItemPortClassification } = {
    "Seat": { group: "Interior", class: "Seat", hideSize: true },
    "Room": { group: "Interior", class: "Room", hideSize: true },
    "Display": { group: "Interior", class: "Display", hideSize: true },
    "ManneuverThruster": { group: "Propulsion", class: "Thruster", hideSize: true },
    "FuelIntake": { group: "Fuel", class: "Intake", hideSize: true },
    "FuelTank": { group: "Fuel", class: "Hydrogen fuel tank", hideSize: true },
    "Cooler": { group: "Systems", class: "Cooler" },
    "Radar": { group: "Sensors", class: "Radar", hideSize: true },
    "QuantumDrive": { group: "Quantum travel", class: "Quantum drive" },
    "Avionics": { group: "Systems", class: "Avionics", hideSize: true },
    "QuantumFuelTank": { group: "Quantum travel", class: "Quantum fuel tank", hideSize: true },
    "TurretBase.MannedTurret": { group: "Offence", class: "Manned turret" },
    "Shield": { group: "Defence", class: "Shield" },
    "MainThruster": { group: "Propulsion", class: "Main engine", hideSize: true },
    "MissileLauncher": { group: "Offence", class: "Missile rack" },
    "WeaponDefensive": { group: "Defence", class: "Unknown" },
    "WeaponDefensive.CountermeasureLauncher": { group: "Defence", class: "Countermeasure launcher", hideSize: true },
    "CoolerController": { group: "Controller", class: "Cooler controller", hideSize: true },
    "ShieldController": { group: "Controller", class: "Shield controller", hideSize: true },
    "EnergyController": { group: "Controller", class: "Energy controller", hideSize: true },
    "WeaponController": { group: "Controller", class: "Weapon controller", hideSize: true },
    "FlightController": { group: "Controller", class: "Flight controller", hideSize: true },
    "CommsController": { group: "Controller", class: "Communications controller", hideSize: true },
    "DoorController": { group: "Controller", class: "Door controller", hideSize: true },
    "LightController": { group: "Controller", class: "Light controller", hideSize: true },
    "Cargo": { group: "Cargo", class: "Cargo grid", hideSize: true },
    "SeatAccess": { group: "Interior", class: "Seat access", hideSize: true },
    "Door": { group: "Interior", class: "Door", hideSize: true },
    "Scanner": { group: "Sensors", class: "Scanner", hideSize: true },
    "Ping": { group: "Sensors", class: "Ping", hideSize: true },
    "Transponder": { group: "Systems", class: "Transponder", hideSize: true },
    "Turret": { group: "Offence", class: "Unknown" },
    "Turret.NoseMounted": { group: "Offence", class: "Turret" },
    "PowerPlant": { group: "Systems", class: "Power plant" },
    "Armor": { group: "Defence", class: "Armor", hideSize: true },
    "SelfDestruct": { group: "Systems", class: "Self destruct", hideSize: true },
    "SeatDashboard": { group: "Interior", class: "Dashboard", hideSize: true },
    "LandingSystem": { group: "Systems", class: "Landing system", hideSize: true },
    "WeaponGun": { group: "Offence", class: "Weapon hardpoint" },
    "Turret.GunTurret": { group: "Offence", class: "Weapon hardpoint" },
    "Turret.MissileTurret": { group: "Offence", class: "Missile turret" },
    "EMP": { group: "Offence", class: "EMP" },
    "Usable": { group: "Usable", class: "Usable item", hideSize: true }
  }

  constructor(private $http: HttpClient, private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.$http.get<Ship>(`${environment.api}/ships/${params.get("name")}.json`).subscribe(r => {
        this.ship = r;

        this.ship.Loadout.forEach(itemPort => itemPort.classification = this.classifyItemPort(itemPort));

        this.grouped = _.groupBy(this.ship!.Loadout, (x: ItemPort) => x.classification.group);

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

  classifyItemPort(itemPort: ItemPort): ItemPortClassification {
    let classification: ItemPortClassification = { type: "unknown", class: "unknown", group: "unknown" };
    if (!itemPort.types) return classification;

    Object.keys(itemPort.types).some(type => {

      let found: ItemPortClassification | undefined;

      // Look it up in the type map
      if (this.typeMap[type]) found = this.typeMap[type];
      else {
        let major = type.split(".")[0];
        if (this.typeMap[major]) found = this.typeMap[major];
      }

      if (found) classification = found;
      return !!found;
    });

    return classification;
  }


}

interface Ship {
  Loadout: ItemPort[];
}

interface ItemPortClassification {
  type?: string;
  class: string;
  group: string;
  hideSize?: boolean;
}

interface ItemPort {
  types: {};
  flags: {};
  classification: ItemPortClassification;
}
