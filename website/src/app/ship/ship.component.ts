import { Component, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import * as _ from "lodash";

import { environment } from "../../environments/environment";
import { ActivatedRoute } from '@angular/router';
import { Ship } from '../Ship';
import { ItemPortClassification } from '../ItemPortClassification';
import { ItemPortLoadout } from '../ItemPortLoadout';
import { SCItem } from '../SCItem';
import { ItemPort } from '../ItemPort';
import { IItemPort } from "../IItemPort";
import { JsonLoadout } from '../JsonLoadout';

interface ClassifiedItemPort {
  classification: ItemPortClassification;
  itemPort: IItemPort;
}

@Component({
  selector: 'app-ship',
  templateUrl: './ship.component.html',
  styleUrls: ['./ship.component.scss']
})
export class ShipComponent implements OnInit {

  ship: Ship | undefined;
  grouped: { [id: string]: any } = {};

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
    "WeaponDefensive.CountermeasureLauncher": { category: "Defence", kind: "Countermeasure", hideSize: true },
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
    "Transponder": { category: "Systems", kind: "Transponder", hideSize: true, isBoring: true },
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

  leftGroups: string[] = [
    "Offence",
    "Defence",
    "Systems"
  ];

  rightGroups: string[] = [
    "Cargo",
    "Propulsion",
    "Quantum travel",
    "Sensors"
  ];

  includeBoring: boolean = false;

  ItemPorts: IItemPort[] = [];

  private itemCache: { [id: string]: any } = {}

  constructor(private $http: HttpClient, private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.$http.get<any>(`${environment.api}/ships/${params.get("name")}.json`).subscribe(async r => {
        this.ship = new Ship(r.Loadout, r.Raw);
        console.log("Loaded ship", this.ship.className);


        console.log("Initialising loadout");
        var vehiclePorts = this.ship.findItemPorts(ip => ip instanceof ItemPort);
        var loadout: JsonLoadout[] | undefined = _.get(r.Raw, "DefaultLoadout.Items", []);
        if (vehiclePorts.length && loadout) await this.loadItems(vehiclePorts, loadout);
        console.log("Loadout initialised");

        this.ItemPorts = this.ship.findItemPorts(ip => ip.types.length > 0);

        // Classify each Item Port
        let classifiedPorts: ClassifiedItemPort[] = [];
        classifiedPorts = _.map(vehiclePorts, (x) => <ClassifiedItemPort>{ itemPort: x, classification: this.classifyItemPort(x) })

        // Filter out the boring ones
        if (!this.includeBoring) classifiedPorts = _.filter(classifiedPorts, x => !x.classification.isBoring);

        // Group by the major grouping
        this.grouped = _.groupBy(classifiedPorts, x => x.classification.category);

        // Secondary group by the class
        _.forEach(this.grouped, (value, key) => this.grouped[key] = _.groupBy(this.grouped[key], x => x.classification.kind))

        // Create an array of ItemPort[] arrays, one for each size 0-9 and add each Item Port to the appropriate array according to maxsize
        let largestSize = _.reduce(classifiedPorts, (max, itemPort) => itemPort.itemPort.maxSize > max ? itemPort.itemPort.maxSize : max, 0);
        if (largestSize < 9) largestSize = 9;
        _.forEach(this.grouped, (gv, gk) => _.forEach(gv, (cv: ClassifiedItemPort[], ck) => {
          let counts: ClassifiedItemPort[][] = [];
          for (let i = 0; i <= largestSize; i++) counts.push([]);

          cv.forEach(itemPort => counts[itemPort.itemPort.maxSize || 0].push(itemPort));
          this.grouped[gk][ck] = { bySize: counts };
        }));

        console.log("Grouped loadout:", this.grouped);
      });
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
    return Object.keys(this.grouped).filter(g => !this.leftGroups.includes(g) && !this.rightGroups.includes(g));
  }

  async loadItems(itemPorts: IItemPort[], loadouts: JsonLoadout[]): Promise<void> {
    for (let i = 0; i < itemPorts.length; i++) {
      let itemPort = itemPorts[i];
      let loadout: JsonLoadout | undefined = _.find(loadouts, x => x.portName == itemPort.name);
      if (loadout && loadout.itemName) {
        itemPort.itemClass = loadout.itemName;
        itemPort.item = await this.loadItem(loadout.itemName);
        if (itemPort.item) {
          let subPorts = itemPort.item.findItemPorts();
          if (subPorts.length && loadout.Items) {
            await this.loadItems(subPorts, loadout.Items);
          }
        }
      }
    }
  }

  async loadItem(itemName: string): Promise<SCItem | undefined> {
    let loaded: any;

    if (itemName) {
      if (this.itemCache[itemName]) loaded = this.itemCache[itemName];
      else {
        loaded = await this.$http.get<any>(`${environment.api}/items/${itemName.toLowerCase()}.json`).toPromise().catch(e => { });
        console.log(loaded ? "Loaded item" : "Could not load item", itemName);
        if (loaded) this.itemCache[itemName] = loaded;
      }
    }

    if (!loaded) return undefined;

    // Clone so that each itemPort gets a unique object
    return new SCItem(JSON.parse(JSON.stringify(loaded)));
  }
}
