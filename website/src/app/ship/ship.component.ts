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

  ship: Ship | null = null;
  grouped: { [id: string]: any } = {};
  types: string[] = [];

  private typeMap: { [id: string]: ItemPortClassification } = {
    "Seat": { category: "Interior", kind: "Seat / Bed", hideSize: true, isBoring: true },
    "Room": { category: "Interior", kind: "Room", hideSize: true, isBoring: true },
    "Display": { category: "Interior", kind: "Display", hideSize: true, isBoring: true },
    "ManneuverThruster": { category: "Propulsion", kind: "Thruster", hideSize: true },
    "FuelIntake": { category: "Propulsion", kind: "Fuel intake", hideSize: true },
    "FuelTank": { category: "Propulsion", kind: "Hydrogen fuel tank", hideSize: true },
    "Cooler": { category: "Systems", kind: "Cooler" },
    "Radar": { category: "Sensors", kind: "Radar", hideSize: true },
    "QuantumDrive": { category: "Quantum travel", kind: "Quantum drive" },
    "Avionics": { category: "Systems", kind: "Avionics", hideSize: true, isBoring: true },
    "QuantumFuelTank": { category: "Quantum travel", kind: "Quantum fuel tank", hideSize: true },
    "TurretBase.MannedTurret": { category: "Offence", kind: "Manned turret" },
    "Shield": { category: "Defence", kind: "Shield" },
    "MainThruster": { category: "Propulsion", kind: "Main engine", hideSize: true },
    "MissileLauncher": { category: "Offence", kind: "Missile rack" },
    "WeaponDefensive": { category: "Defence", kind: "Unknown" },
    "WeaponDefensive.CountermeasureLauncher": { category: "Defence", kind: "Countermeasure launcher", hideSize: true },
    "CoolerController": { category: "Controllers", kind: "Cooler controller", hideSize: true, isBoring: true },
    "ShieldController": { category: "Controllers", kind: "Shield controller", hideSize: true, isBoring: true },
    "EnergyController": { category: "Controllers", kind: "Energy controller", hideSize: true, isBoring: true },
    "WeaponController": { category: "Controllers", kind: "Weapon controller", hideSize: true, isBoring: true },
    "FlightController": { category: "Controllers", kind: "Flight controller", hideSize: true, isBoring: true },
    "CommsController": { category: "Controllers", kind: "Communications controller", hideSize: true, isBoring: true },
    "DoorController": { category: "Controllers", kind: "Door controller", hideSize: true, isBoring: true },
    "LightController": { category: "Controllers", kind: "Light controller", hideSize: true, isBoring: true },
    "WheeledController": { category: "Controllers", kind: "Wheeled controller", hideSize: true, isBoring: true },
    "Cargo": { category: "Cargo", kind: "Cargo grid", hideSize: true },
    "SeatAccess": { category: "Interior", kind: "Seat access", hideSize: true, isBoring: true },
    "Door": { category: "Interior", kind: "Door", hideSize: true, isBoring: true },
    "Scanner": { category: "Sensors", kind: "Scanner", hideSize: true },
    "Ping": { category: "Sensors", kind: "Ping", hideSize: true, isBoring: true },
    "Transponder": { category: "Systems", kind: "Transponder", hideSize: true },
    "Turret": { category: "Offence", kind: "Unknown" },
    "Turret.NoseMounted": { category: "Offence", kind: "Turret" },
    "PowerPlant": { category: "Systems", kind: "Power plant" },
    "Armor": { category: "Defence", kind: "Armor", hideSize: true },
    "SelfDestruct": { category: "Systems", kind: "Self destruct", hideSize: true, isBoring: true },
    "SeatDashboard": { category: "Interior", kind: "Dashboard", hideSize: true, isBoring: true },
    "LandingSystem": { category: "Systems", kind: "Landing system", hideSize: true, isBoring: true },
    "WeaponGun": { category: "Offence", kind: "Weapon hardpoint" },
    "Turret.GunTurret": { category: "Offence", kind: "Weapon hardpoint" },
    "Turret.MissileTurret": { category: "Offence", kind: "Missile turret" },
    "Turret.CanardTurret": { category: "Offence", kind: "Weapon hardpoint" },
    "Turret.BallTurret": { category: "Offence", kind: "Turret" },
    "EMP": { category: "Offence", kind: "EMP" },
    "Usable": { category: "Usables", kind: "Usable item", hideSize: true, isBoring: true },
    "QuantumInterdictionGenerator": { category: "Offence", kind: "Quantum Interdiction" }
  }

  includeBoring: boolean = false;

  constructor(private $http: HttpClient, private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.$http.get<Ship>(`${environment.api}/ships/${params.get("name")}.json`).subscribe(r => {
        this.ship = r;

        // Classify each Item Port
        this.ship.Loadout.forEach(itemPort => itemPort.classification = this.classifyItemPort(itemPort));

        // Filter out the boring ones
        if (!this.includeBoring) this.ship.Loadout = _.filter(this.ship.Loadout, x => !x.classification.isBoring);

        // Group by the major grouping
        this.grouped = _.groupBy(this.ship!.Loadout, (x: ItemPort) => x.classification.category);

        // _.forEach(this.grouped, (value, key) => this.grouped[key] = _.orderBy(this.grouped[key], ["grouping.type", "maxsize", "port"], ["asc", "desc", "asc"]));

        // Secondary group by the class
        _.forEach(this.grouped, (value, key) => this.grouped[key] = _.groupBy(this.grouped[key], (x: ItemPort) => x.classification.kind))

        // Create an array of 10 ItemPort[] arrays, one for each size 0-9 and add each Item Port to the appropriate array according to maxsize
        _.forEach(this.grouped, (gv, gk) => _.forEach(gv, (cv: ItemPort[], ck) => {
          let counts: ItemPort[][] = [[], [], [], [], [], [], [], [], [], []];

          cv.forEach(itemPort => counts[itemPort.maxsize || 0].push(itemPort));
          this.grouped[gk][ck] = counts;
        }));

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
    if (!itemPort.types) return { category: "Unknown", kind: itemPort.port || "Unknown" };

    let classification: ItemPortClassification | undefined;

    Object.keys(itemPort.types).some(type => {
      if (this.typeMap[type]) classification = this.typeMap[type];
      return !!classification;
    });
    if (classification) return classification;

    Object.keys(itemPort.types).some(type => {
      let major = type.split(".")[0];
      if (this.typeMap[major]) classification = this.typeMap[major];
      return !!classification;
    });

    if (classification) return classification;

    return { category: "Unknown", kind: Object.keys(itemPort.types)[0] };
  }
}

interface Ship {
  Loadout: ItemPort[];
}

interface ItemPortClassification {
  category: string;
  kind: string;
  hideSize?: boolean;
  isBoring?: boolean;
}

interface ItemPort {
  types: {};
  flags: {};
  classification: ItemPortClassification;
  minsize: number;
  maxsize: number;
  item: string;
  port: string;
}
