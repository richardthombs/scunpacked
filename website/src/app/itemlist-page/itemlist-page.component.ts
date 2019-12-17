import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as _ from "lodash";

import { environment } from "../../environments/environment";
import { doubleGroupedList } from "../shiplist-page/shiplist-page.component";

@Component({
  selector: 'app-itemlist',
  templateUrl: './itemlist-page.component.html',
  styleUrls: ['./itemlist-page.component.scss']
})
export class ItemlistPage implements OnInit {

  constructor(private $http: HttpClient) { }

  byType: doubleGroupedList<ItemIndexEntry> = {}

  selectedType: any;

  typeMap: { [id: string]: string } = {
    "Armor.Light": "Armor",
    "Armor.Medium": "Armor",
    "Cooler.UNDEFINED": "Coolers",
    "EMP.UNDEFINED": "EMPs",
    "Missile.Missile": "Missiles",
    "Missile.Torpedo": "Torpedos",
    "Missile.Rocket": "Rockets",
    "PowerPlant.Power": "Power plants",
    "QuantumDrive.UNDEFINED": "Quantum drives",
    "QuantumInterdictionGenerator.UNDEFINED": "Quantum interdiction",
    "Radar.MidRangeRadar": "Radars",
    "Scanner.Scanner": "Scanners",
    "Scanner.UNDEFINED": "Scanners",
    "Shield.UNDEFINED": "Shields",
    "WeaponDefensive.CountermeasureLauncher": "Countermeasures",
    "WeaponGun.Gun": "Weapons",
    "WeaponGun.Rocket": "Rocket launchers",
    "WeaponGun.NoseMounted": "Weapons",
    "WeaponMining.Gun": "Mining lasers"
  }

  ngOnInit() {

    this.$http.get<ItemIndexEntry[]>(`${environment.api}/items.json`).subscribe(r => {

      r = r.filter(x => x.type);
      r = r.filter(x => !x.type.includes("AIModule"));
      r = r.filter(x => !x.type.includes("Battery"));
      r = r.filter(x => !x.type.includes("Button"));
      r = r.filter(x => !x.type.includes("Cargo"));
      r = r.filter(x => !x.type.includes("Controller"));
      r = r.filter(x => !x.type.includes("ControlPanel"));
      r = r.filter(x => !x.type.includes("Container"));
      r = r.filter(x => !x.type.includes("Display"));
      r = r.filter(x => !x.type.includes("Door"));
      r = r.filter(x => !x.type.includes("FuelIntake"));
      r = r.filter(x => !x.type.includes("FuelTank"));
      r = r.filter(x => !x.type.includes("LandingSystem"));
      r = r.filter(x => !x.type.includes("Lightgroup"));
      r = r.filter(x => !x.type.includes("MainThruster"));
      r = r.filter(x => !x.type.includes("ManneuverThruster"));
      r = r.filter(x => !x.type.includes("MiningArm"));
      r = r.filter(x => !x.type.includes("MiningModifier"));
      r = r.filter(x => !x.type.includes("Misc"));
      r = r.filter(x => !x.type.includes("MissileLauncher"));
      r = r.filter(x => !x.type.includes("Paints"));
      r = r.filter(x => !x.type.includes("Ping"));
      r = r.filter(x => !x.type.includes("Player"));
      r = r.filter(x => !x.type.includes("QuantumFuelTank"));
      r = r.filter(x => !x.type.includes("Seat"));
      r = r.filter(x => !x.type.includes("SeatAccess"));
      r = r.filter(x => !x.type.includes("SeatDashboard"));
      r = r.filter(x => !x.type.includes("SelfDestruct"));
      r = r.filter(x => !x.type.includes("Sensor"));
      r = r.filter(x => !x.type.includes("TargetSelector"));
      r = r.filter(x => !x.type.includes("Turret"));
      r = r.filter(x => !x.type.includes("TurretBase"));
      r = r.filter(x => !x.type.includes("UNDEFINED"));
      r = r.filter(x => !x.type.includes("Usable"));
      r = r.filter(x => !x.type.includes("WeaponAttachment"));

      // Group by role and sub-role, ships will appear in multiple groupings
      r.forEach(i => {
        let type = this.typeMap[`${i.type || "UNDEFINED"}.${i.subType || "UNDEFINED"}`] || "Unknown";

        let size = `Size ${i.size || 0}`;
        let manu = i.manufacturer || "CIG";

        let level1 = type;
        let level2 = size;

        if (!this.byType[level1]) this.byType[level1] = {};
        if (!this.byType[level1][level2]) this.byType[level1][level2] = [];
        this.byType[level1][level2].push(i);
      });

      _.forEach(this.byType, l1 => _.forEach(l1, (v2, k2) => l1[k2] = _.sortBy(v2, x => x.manufacturer || "CIG")));

      var first = _.sortBy(Object.keys(this.byType))[0];

      this.selectType({ key: first, value: this.byType[first] });
    });

  }

  itemsInRole(level1: { [id: string]: ItemIndexEntry[] }): string {
    let items: string[] = [];
    _.map(level1, level2 => items = items.concat(_.map(level2, s => s.className.toLowerCase())));
    return items.join(",");
  }

  itemsInSubRole(indexEntry: ItemIndexEntry[]): string {
    return _.map(indexEntry, s => s.className.toLowerCase()).join(",");
  }

  selectType(type: any) {
    console.log(type);
    this.selectedType = type;
  }
}

interface ItemIndexEntry {
  name: string;
  type: string;
  subType: string;
  size: number | undefined;
  grade: number | undefined;
  manufacturer: string;
  className: string;
}
