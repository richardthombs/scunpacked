import { Component, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import * as _ from "lodash";

import { environment } from "../../environments/environment";
import { ActivatedRoute } from '@angular/router';
import { Ship } from '../Ship';
import { ItemPortClassification } from '../ItemPortClassification';
import { ItemPortLoadout } from '../ItemPortLoadout';
import { Item } from '../Item';
import { SCItem } from '../SCItem';
import { IItemPort } from '../ItemPort';

@Component({
  selector: 'app-ship',
  templateUrl: './ship.component.html',
  styleUrls: ['./ship.component.scss']
})
export class ShipComponent implements OnInit {

  ship: Ship | undefined;
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

  private itemCache: { [id: string]: Item } = {}

  constructor(private $http: HttpClient, private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.$http.get<any>(`${environment.api}/ships/${params.get("name")}.json`).subscribe(async r => {

        let ship = r;

        console.log("Loaded ship:", ship);

        this.ship = new Ship(r.Loadout, r.Raw);

        // Load each item
        for (let i = 0; i < ship.Loadout.length; i++) {
          var itemport = ship.Loadout[i];
          if (itemport.item) {
            var item = await this.loadItem(itemport.item);
            if (!item) console.log("Can't load item:", itemport.item);
            else {
              itemport.loadedItem = new SCItem(item);
              let ip = this.ship.findItemPorts(x => x.name == itemport.port);
              if (ip.length) ip[0].item = new SCItem(item);
              else console.log("Can't find IItemPort with name ", itemport.port);
            }
          }
        };

        this.ItemPorts = this.ship.findItemPorts(ip => ip.types.length > 0);


        // Classify each Item Port
        this.ship.Loadout.forEach(itemPort => itemPort.classification = this.classifyItemPort(itemPort));

        // Filter out the boring ones
        if (!this.includeBoring) this.ship.Loadout = _.filter(this.ship.Loadout, x => !x.classification.isBoring);

        // Group by the major grouping
        this.grouped = _.groupBy(this.ship!.Loadout, (x: ItemPortLoadout) => x.classification.category);

        // Secondary group by the class
        _.forEach(this.grouped, (value, key) => this.grouped[key] = _.groupBy(this.grouped[key], (x: ItemPortLoadout) => x.classification.kind))

        // Create an array of ItemPort[] arrays, one for each size 0-9 and add each Item Port to the appropriate array according to maxsize
        let largestSize = _.reduce(this.ship.Loadout, (max, itemPort) => itemPort.maxsize > max ? itemPort.maxsize : max, 0);
        if (largestSize < 9) largestSize = 9;
        _.forEach(this.grouped, (gv, gk) => _.forEach(gv, (cv: ItemPortLoadout[], ck) => {
          let counts: ItemPortLoadout[][] = [];
          for (let i = 0; i <= largestSize; i++) counts.push([]);

          cv.forEach(itemPort => counts[itemPort.maxsize || 0].push(itemPort));
          this.grouped[gk][ck] = { bySize: counts };
        }));

        console.log("Grouped loadout:", this.grouped);

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

  private classifyItemPort(itemPort: ItemPortLoadout): ItemPortClassification {
    if (!itemPort.types) return { category: "Unknown", kind: itemPort.port || "Unknown", isBoring: true };

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

    return { category: "Unknown", kind: Object.keys(itemPort.types)[0], isBoring: true };
  }

  unexpectedGroups() {
    return Object.keys(this.grouped).filter(g => !this.leftGroups.includes(g) && !this.rightGroups.includes(g));
  }

  async loadItem(itemName: string): Promise<Item | undefined> {
    var loaded = undefined;
    if (itemName) {
      if (this.itemCache[itemName]) loaded = this.itemCache[itemName];
      else {
        loaded = await this.$http.get<Item>(`${environment.api}/items/${itemName.toLowerCase()}.json`).toPromise().catch(e => { });
        if (loaded) this.itemCache[itemName] = loaded;
      }
    }

    if (loaded) return Promise.resolve(JSON.parse(JSON.stringify(loaded)));
    return Promise.resolve(undefined);
  }
}
