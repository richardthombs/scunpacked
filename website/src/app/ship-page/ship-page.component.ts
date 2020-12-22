import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as _ from "lodash";

import { Ship, StandardisedItemPort } from '../Ship';
import { ItemPortClassification } from '../ItemPortClassification';
import { IItemPort } from "../IItemPort";
import { ShipService } from '../ship.service';
import { environment } from '../../environments/environment';
import { Title } from '@angular/platform-browser';
import { LocalisationService } from '../Localisation';

interface ClassifiedItemPort {
  classification: ItemPortClassification;
  itemPort: StandardisedItemPort;
}

@Component({
  selector: 'app-ship',
  templateUrl: './ship-page.component.html'
})
export class ShipPage implements OnInit {

  private typeMap: { [id: string]: ItemPortClassification } = {
    "Main thrusters": { category: "Propulsion", kind: "Main engines" },
    "Retro thrusters": { category: "Propulsion", kind: "Retro thrusters" },
    "VTOL thrusters": { category: "Propulsion", kind: "VTOL thrusters" },
    "Maneuvering thrusters": { category: "Propulsion", kind: "Maneuvering thrusters" },
    "Fuel intakes": { category: "Propulsion", kind: "Fuel intakes" },
    "Fuel tanks": { category: "Propulsion", kind: "Hydrogen fuel tanks" },

    "Coolers": { category: "Systems", kind: "Coolers" },
    "Power plants": { category: "Systems", kind: "Power plants" },

    "Weapon hardpoints": { category: "Offence", kind: "Weapon hardpoints" },
    "Manned turrets": { category: "Offence", kind: "Manned turrets" },
    "Remote turrets": { category: "Offence", kind: "Remote turrets" },
    "Missile racks": { category: "Offence", kind: "Missile racks" },
    "EMP hardpoints": { category: "Offence", kind: "EMP generators" },
    "QIG hardpoints": { category: "Offence", kind: "Quantum interdiction generators" },

    "Mining hardpoints": { category: "Utility", kind: "Mining hardpoints" },
    "Mining turrets": { category: "Utility", kind: "Mining turrets" },
    "Utility hardpoints": { category: "Utility", kind: "Utility hardpoints" },

    "Cargo grids": { category: "Cargo", kind: "Cargo grids" },
    "Cargo containers": { category: "Cargo", kind: "Cargo containers" },

    "Quantum drives": { category: "Quantum travel", kind: "Quantum drives" },
    "Quantum fuel tanks": { category: "Quantum travel", kind: "Quantum fuel tanks" },

    "Shield generators": { category: "Defence", kind: "Shield generators" },
    "Countermeasures": { category: "Defence", kind: "Countermeasures" },

    "Radars": { category: "Avionics", kind: "Radars" },
    "Pings": { category: "Avionics", kind: "Pings" },
    "Scanners": { category: "Avionics", kind: "Scanners" },
    "Transponders": { category: "Avionics", kind: "Transponders" }
  }

  groups: string[] = [
    "Offence",
    "Defence",
    "Utility",
    "Systems",
    "Avionics",
    "Cargo",
    "Propulsion",
    "Quantum travel",
  ];

  includeBoring: boolean = true;

  ship: Ship | undefined;
  grouped: { [id: string]: any } = {};
  ItemPorts: IItemPort[] = [];
  jsonHref: string = "";
  totalDamageBeforeDestruction: number = 0;
  totalShieldHealth: number = 0;
  totalShieldRegen: number = 0;
  quantumDriveSpeed: number = 0;
  quantumDriveRange: number = 0;

  constructor(private shipSvc: ShipService, private route: ActivatedRoute, private titleSvc: Title, private localisationSvc: LocalisationService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {

      let shipClass = params.get("name");
      if (!shipClass) return;

      this.jsonHref = `${environment.api}/v2/ships/${shipClass}.json`;

      this.shipSvc.load(shipClass).then(loaded => {

        let ship = loaded.Ship;
        let ports = loaded.Ports;

        this.titleSvc.setTitle(`${ship.Name}`);

        let vehiclePorts: StandardisedItemPort[] = [];

        // Hacky way to get all the ports into a single list. Maybe API needs a list of ports as well as a grouping
        vehiclePorts.push(...ports.PilotHardpoints);
        vehiclePorts.push(...ports.MannedTurrets);
        vehiclePorts.push(...ports.RemoteTurrets);
        vehiclePorts.push(...ports.MiningTurrets);
        vehiclePorts.push(...ports.UtilityTurrets);
        vehiclePorts.push(...ports.MiningHardpoints);
        vehiclePorts.push(...ports.UtilityHardpoints);
        vehiclePorts.push(...ports.MissileRacks);
        vehiclePorts.push(...ports.Countermeasures);
        vehiclePorts.push(...ports.Shields);
        vehiclePorts.push(...ports.PowerPlants);
        vehiclePorts.push(...ports.Coolers);
        vehiclePorts.push(...ports.QuantumDrives);
        vehiclePorts.push(...ports.QuantumFuelTanks);
        vehiclePorts.push(...ports.MainThrusters);
        vehiclePorts.push(...ports.RetroThrusters);
        vehiclePorts.push(...ports.VtolThrusters);
        vehiclePorts.push(...ports.ManeuveringThrusters);
        vehiclePorts.push(...ports.HydrogenFuelTanks);
        vehiclePorts.push(...ports.HydogenFuelIntakes);
        vehiclePorts.push(...ports.InterdictionHardpoints);
        vehiclePorts.push(...ports.CargoGrids);
        vehiclePorts.push(...ports.Avionics);

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
        let largestSize = _.reduce(classifiedPorts, (max, itemPort) => itemPort.itemPort.Size > max ? itemPort.itemPort.Size : max, 0);
        if (largestSize < 9) largestSize = 9;

        // Create an array of ItemPort[] arrays, one for each size 0-9 and add each Item Port to the appropriate array according to maxsize
        _.forEach(grouped, (gv, gk) => _.forEach(gv, (cv: ClassifiedItemPort[], ck) => {
          let counts: ClassifiedItemPort[][] = [];
          for (let i = 0; i <= largestSize; i++) counts.push([]);
          cv.forEach(itemPort => counts[itemPort.itemPort.Size || 0].push(itemPort));
          grouped[gk][ck] = { bySize: counts };
        }));

        this.ship = ship;
        this.grouped = grouped;

        this.totalShieldHealth = _.sumBy(ports.Shields, "InstalledItem.Shield.Health");
        this.totalShieldRegen = _.sumBy(ports.Shields, "InstalledItem.Shield.Regeneration");
        this.quantumDriveSpeed = ports.QuantumDrives.length ? ports.QuantumDrives[0].InstalledItem.QuantumDrive.StandardJump.Speed : 0;
        this.quantumDriveRange = ports.QuantumDrives.length ? ports.QuantumDrives[0].InstalledItem.QuantumDrive.JumpRange : 0;

        console.log("Ship", this.ship);
        console.log("ItemPorts", this.ItemPorts);
        console.log("Loadout:", this.grouped);

      })
    });
  }

  private classifyItemPort(itemPort: StandardisedItemPort): ItemPortClassification {
    if (!itemPort.Types.length) return { category: "Unknown", kind: itemPort.PortName || "Unknown", isBoring: true };

    if (this.typeMap[itemPort.Category]) return this.typeMap[itemPort.Category];

    return { category: "Unknown", kind: itemPort.Category, isBoring: true };
  }

  unexpectedGroups() {
    return Object.keys(this.grouped).filter(g => !this.groups.includes(g));
  }

  toggleBoring() {
    this.includeBoring = !this.includeBoring;
  }

}
