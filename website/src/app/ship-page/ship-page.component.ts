import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as _ from "lodash";

import { Ship } from '../Ship';
import { ItemPortClassification } from '../ItemPortClassification';
import { ItemPort } from '../ItemPort';
import { IItemPort } from "../IItemPort";
import { ShipService } from '../ship.service';

interface ClassifiedItemPort {
  classification: ItemPortClassification;
  itemPort: IItemPort;
}

@Component({
  selector: 'app-ship',
  templateUrl: './ship-page.component.html',
  styleUrls: ['./ship-page.component.scss']
})
export class ShipPage implements OnInit {

  private typeMap: { [id: string]: ItemPortClassification } = {
    "Seat": { category: "Interior", kind: "Seat / Bed", isBoring: true },
    "Room": { category: "Interior", kind: "Room", isBoring: true },
    "Display": { category: "Interior", kind: "Display", isBoring: true },
    "ManneuverThruster": { category: "Propulsion", kind: "Thruster" },
    "FuelIntake": { category: "Propulsion", kind: "Fuel intake" },
    "FuelTank": { category: "Propulsion", kind: "Hydrogen fuel tank" },
    "Cooler": { category: "Systems", kind: "Cooler" },
    "Radar": { category: "Sensors", kind: "Radar" },
    "QuantumDrive": { category: "Quantum travel", kind: "Quantum drive" },
    "Avionics": { category: "Systems", kind: "Avionics", isBoring: true },
    "QuantumFuelTank": { category: "Quantum travel", kind: "Quantum fuel tank" },
    "TurretBase.MannedTurret": { category: "Offence", kind: "Manned turret" },
    "Shield": { category: "Defence", kind: "Shield" },
    "MainThruster": { category: "Propulsion", kind: "Main engine" },
    "MissileLauncher": { category: "Offence", kind: "Missile rack" },
    "WeaponDefensive": { category: "Defence", kind: "Unknown" },
    "WeaponDefensive.CountermeasureLauncher": { category: "Defence", kind: "Countermeasure" },
    "CoolerController": { category: "Controllers", kind: "Cooler controller", isBoring: true },
    "ShieldController": { category: "Controllers", kind: "Shield controller", isBoring: true },
    "EnergyController": { category: "Controllers", kind: "Energy controller", isBoring: true },
    "WeaponController": { category: "Controllers", kind: "Weapon controller", isBoring: true },
    "FlightController": { category: "Controllers", kind: "Flight controller", isBoring: true },
    "CommsController": { category: "Controllers", kind: "Communications controller", isBoring: true },
    "DoorController": { category: "Controllers", kind: "Door controller", isBoring: true },
    "LightController": { category: "Controllers", kind: "Light controller", isBoring: true },
    "WheeledController": { category: "Controllers", kind: "Wheeled controller", isBoring: true },
    "MiningController": { category: "Controllers", kind: "Mining controller", isBoring: true },
    "Cargo": { category: "Cargo", kind: "Cargo grid" },
    "SeatAccess": { category: "Interior", kind: "Seat access", isBoring: true },
    "Door": { category: "Interior", kind: "Door", isBoring: true },
    "Scanner": { category: "Sensors", kind: "Scanner" },
    "Ping": { category: "Sensors", kind: "Ping", isBoring: true },
    "Transponder": { category: "Systems", kind: "Transponder", isBoring: true },
    "Turret": { category: "Offence", kind: "Unknown" },
    "Turret.NoseMounted": { category: "Offence", kind: "Turret" },
    "PowerPlant": { category: "Systems", kind: "Power plant" },
    "Armor": { category: "Defence", kind: "Armor" },
    "SelfDestruct": { category: "Systems", kind: "Self destruct", isBoring: true },
    "SeatDashboard": { category: "Interior", kind: "Dashboard", isBoring: true },
    "LandingSystem": { category: "Systems", kind: "Landing system", isBoring: true },
    "WeaponGun": { category: "Offence", kind: "Weapon hardpoint" },
    "Turret.GunTurret": { category: "Offence", kind: "Weapon hardpoint" },
    "Turret.MissileTurret": { category: "Offence", kind: "Missile turret" },
    "Turret.CanardTurret": { category: "Offence", kind: "Weapon hardpoint" },
    "Turret.BallTurret": { category: "Offence", kind: "Turret" },
    "EMP": { category: "Offence", kind: "EMP" },
    "Usable": { category: "Usables", kind: "Usable item", isBoring: true },
    "QuantumInterdictionGenerator": { category: "Offence", kind: "Quantum Interdiction" },
    "MiningArm": { category: "Offence", kind: "Mining arm" },
    "Container.Cargo": { category: "Cargo", kind: "Cargo container" },
  }

  groups: string[] = [
    "Offence",
    "Defence",
    "Systems",
    "Cargo",
    "Propulsion",
    "Quantum travel",
    "Sensors"
  ];

  includeBoring: boolean = true;

  ship: Ship | undefined;
  grouped: { [id: string]: any } = {};
  ItemPorts: IItemPort[] = [];

  constructor(private shipSvc: ShipService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {

      let shipClass = params.get("name");
      if (!shipClass) return;

      this.shipSvc.load(shipClass).then(ship => {

        let vehiclePorts = ship.findItemPorts(ip => ip instanceof ItemPort);

        // Classify each Item Port
        let classifiedPorts: ClassifiedItemPort[] = [];
        classifiedPorts = _.map(vehiclePorts, (x) => <ClassifiedItemPort>{ itemPort: x, classification: this.classifyItemPort(x) })

        // Filter out the boring ones
        if (!this.includeBoring) classifiedPorts = _.filter(classifiedPorts, x => !x.classification.isBoring);

        // Group by the major grouping
        let grouped: { [id: string]: any } = _.groupBy(classifiedPorts, x => x.classification.category);

        // Secondary group by the class
        _.forEach(grouped, (value, key) => grouped[key] = _.groupBy(grouped[key], x => x.classification.kind))

        // Figure out what the largest size port is
        let largestSize = _.reduce(classifiedPorts, (max, itemPort) => itemPort.itemPort.maxSize > max ? itemPort.itemPort.maxSize : max, 0);
        if (largestSize < 9) largestSize = 9;

        // Create an array of ItemPort[] arrays, one for each size 0-9 and add each Item Port to the appropriate array according to maxsize
        _.forEach(grouped, (gv, gk) => _.forEach(gv, (cv: ClassifiedItemPort[], ck) => {
          let counts: ClassifiedItemPort[][] = [];
          for (let i = 0; i <= largestSize; i++) counts.push([]);
          cv.forEach(itemPort => counts[itemPort.itemPort.maxSize || 0].push(itemPort));
          grouped[gk][ck] = { bySize: counts };
        }));

        this.ship = ship;
        this.ItemPorts = ship.findItemPorts();
        this.grouped = grouped;

        console.log("Ship", this.ship);
        console.log("ItemPorts", this.ItemPorts);
        console.log("Loadout:", this.grouped);

      })
    });
  }

  private classifyItemPort(itemPort: IItemPort): ItemPortClassification {
    if (!itemPort.types.length) return { category: "Unknown", kind: itemPort.name || "Unknown", isBoring: true };

    let classification: ItemPortClassification | undefined;

    itemPort.types.some(type => {
      if (this.typeMap[type]) classification = this.typeMap[type];
      return !!classification;
    });
    if (classification) return classification;

    itemPort.types.some(type => {
      let major = type.split(".")[0];
      if (this.typeMap[major]) classification = this.typeMap[major];
      return !!classification;
    });

    if (classification) return classification;

    return { category: "Unknown", kind: itemPort.types[0], isBoring: true };
  }

  unexpectedGroups() {
    return Object.keys(this.grouped).filter(g => !this.groups.includes(g));
  }

  toggleBoring() {
    this.includeBoring = !this.includeBoring;
  }

}
