import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import * as _ from "lodash";

import { doubleGroupedList } from "../shiplist-page/shiplist-page.component";
import { ItemIndexEntry } from '../ItemIndexEntry';

@Component({
  selector: 'app-itemlist',
  templateUrl: './itemlist-page.component.html'
})
export class ItemlistPage implements OnInit {

  constructor(private route: ActivatedRoute, private router: Router) { }

  byType: doubleGroupedList<ItemIndexEntry> = {}

  subs: Subscription[] = [];

  selectedType: any;

  typeMap: { [id: string]: string } = {
    "Ship.Armor.Light": "Armor",
    "Ship.Armor.Medium": "Armor",
    "Ship.Cooler": "Coolers",
    "Ship.EMP": "EMP",
    "Ship.Mining.Gun": "Mining",
    "Ship.MissileLauncher.MissileRack": "Missile racks",
    "Ship.Missile.Missile": "Missiles",
    "Ship.Missile.Rocket": "Rockets",
    "Ship.Missile.Torpedo": "Missiles",
    "Ship.PowerPlant.Power": "Power plants",
    "Ship.QuantumDrive": "Quantum drives",
    "Ship.QuantumInterdictionGenerator": "Quantum interdiction",
    "Ship.Radar": "Radars",
    "Ship.Radar.ShortRangeRadar": "Radars",
    "Ship.Radar.MidRangeRadar": "Radars",
    "Ship.Scanner": "Scanners",
    "Ship.Scanner.Scanner": "Scanners",
    "Ship.Ping": "Pings",
    "Ship.Shield": "Shields",
    "Ship.Weapon.Gun": "Weapons",
    "Ship.Weapon.NoseMounted": "Weapons",
    "Ship.Weapon.Rocket": "Rockets",
    "Ship.WeaponAttachment.Barrel": "SKIP",
    "Ship.WeaponAttachment.FiringMechanism": "SKIP",
    "Ship.WeaponAttachment.PowerArray": "SKIP",
    "Ship.WeaponAttachment.Ventilation": "SKIP",
    "Ship.WeaponDefensive.CountermeasureLauncher": "Countermeasures",
    "Ship.Paints.Personal": "Paints",
    "Ship.Paints": "Paints"
  }

  ngOnInit() {

    this.subs.push(this.route.data.subscribe(data => {

      data.items.forEach((i: ItemIndexEntry) => {
        let type = this.typeMap[i.classification] || i.classification;
        if (type == "SKIP") return;

        let size = `Size ${i.size || 0}`;

        let level1 = type;
        let level2 = size;

        if (!this.byType[level1]) this.byType[level1] = {};
        if (!this.byType[level1][level2]) this.byType[level1][level2] = [];
        this.byType[level1][level2].push(i);
      });

      _.forEach(this.byType, l1 => _.forEach(l1, (v2, k2) => l1[k2] = _.sortBy(v2, x => x.manufacturer || "CIG")));
    }));

    this.subs.push(this.route.queryParamMap.subscribe(r => {
      console.log(r);
      let type = r.get("type");
      if (type) this.selectType({ key: type, value: this.byType[type] });
      else this.router.navigateByUrl("/items?type=Armor");

    }));
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
    this.selectedType = type;
  }
}
