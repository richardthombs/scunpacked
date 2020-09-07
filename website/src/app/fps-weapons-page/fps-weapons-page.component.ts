import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import * as _ from 'lodash';

import { doubleGroupedList } from '../shiplist-page/shiplist-page.component';
import { ItemIndexEntry } from '../ItemIndexEntry';

@Component({
  selector: 'app-fps-weapons-page',
  templateUrl: './fps-weapons-page.component.html',
  styles: [
    `
      :host {
        display: block;
      }
    `
  ]
})
export class FpsWeaponsPageComponent implements OnInit {

  constructor(private route: ActivatedRoute, private router: Router) { }

  byType: doubleGroupedList<ItemIndexEntry> = {}

  subs: Subscription[] = [];

  selectedType: any;

  typeMap: { [id: string]: string } = {
    "WeaponPersonal.Small": "Guns",
    "WeaponPersonal.Medium": "Guns",
    "WeaponPersonal.Large": "Guns",
    "WeaponPersonal.Knife": "Knives",
    "WeaponPersonal.Gadget": "Tools",
    "Light.Weapon": "Weapon attachments",
    "WeaponAttachment.IronSights": "Scopes",

    "Armor.Light": "skip",
    "Armor.Medium": "skip",
    "Cooler.UNDEFINED": "skip",
    "EMP.UNDEFINED": "skip",
    "Missile.Missile": "skip",
    "Missile.Torpedo": "skip",
    "Missile.Rocket": "skip",
    "PowerPlant.Power": "skip",
    "QuantumDrive.UNDEFINED": "skip",
    "QuantumInterdictionGenerator.UNDEFINED": "skip",
    "Radar.MidRangeRadar": "skip",
    "Scanner.Scanner": "skip",
    "Scanner.UNDEFINED": "skip",
    "Shield.UNDEFINED": "skip",
    "WeaponDefensive.CountermeasureLauncher": "skip",
    "WeaponGun.Gun": "skip",
    "WeaponGun.Rocket": "skip",
    "WeaponGun.NoseMounted": "skip",
    "WeaponMining.Gun": "skip"
  }

  ngOnInit() {

    this.subs.push(this.route.data.subscribe(data => {

      data.items.forEach((i: ItemIndexEntry) => {
        let type = this.typeMap[`${i.type || "UNDEFINED"}.${i.subType || "UNDEFINED"}`] || "Unknown";
        if (type == "skip") return;

        let size = `Size ${i.size || 0}`;
        let manu = i.manufacturer || "CIG";

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
      else this.router.navigateByUrl("/fps-weapons?type=Guns");

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
