import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import * as _ from 'lodash';

import { environment } from 'src/environments/environment';

import { SCItem } from '../SCItem';
import { LocalisationService } from '../localisation.service';
import { SiPipe } from '../si.pipe';

type FieldValue = number | string | undefined;

class ComparisonEntry {

  title: string = "";
  value: (item: SCItem) => FieldValue = () => undefined;
  format: (value: FieldValue) => string | null = this.defaultFormat;
  units: string = "";
  siPrefix: boolean = false;
  sortDirection: "asc" | "desc" = "asc";
  decimals: number = 0;
  link?: (item: SCItem) => string = undefined;

  constructor(init?: Partial<ComparisonEntry>) {
    Object.assign(this, init);
  }

  formattedValue(item: SCItem): string {
    let v = this.value(item);
    return this.format(v) || "";
  }

  private defaultFormat(v: FieldValue) {
    if (!v) return "";

    let value = v, units = this.units, formatted = v.toString();

    if (this.isNumber(value)) {
      if (isFinite(value)) {
        if (this.siPrefix) {
          let si = SiPipe.siPrefix(value);
          value = si.value;
          units = si.prefix + this.units;
        }
        formatted = value.toFixed(this.decimals);
      }
      else formatted = "∞";
    }

    if (formatted && units) formatted += " " + units;

    return formatted;
  }

  private isNumber(x: any): x is number {
    return typeof x === "number";
  }
}

@Component({
  selector: 'app-compare-items-page',
  templateUrl: './compare-items-page.component.html',
  styleUrls: ['./compare-items-page.component.scss']
})
export class CompareItemsPage implements OnInit {

  fields: ComparisonEntry[] = [
    new ComparisonEntry({ title: "Name", value: i => this.localisationSvc.getText(i.name, i.className), link: i => `/items/${i.className.toLowerCase()}` }),
    new ComparisonEntry({ title: "Size", value: i => i.size, format: v => `Size ${v}` }),
    new ComparisonEntry({ title: "Quantum speed", units: "m/s", siPrefix: true, value: i => i.driveSpeed }),
    new ComparisonEntry({ title: "PO to ArcCorp", units: "s", value: i => i.secondsToArcCorp }),
    // new ComparisonEntry({ title: "Quantum fuel requirement", units: "l/Gm", value: i => i.quantumFuelRequirement * 1e9 }),
    new ComparisonEntry({ title: "Efficiency", units: "m/l", siPrefix: true, value: i => 1 / i.quantumFuelRequirement }),
    new ComparisonEntry({ title: "Standby power draw", units: "W", value: i => _.get(i, "powerConnection.PowerBase") }),
    new ComparisonEntry({ title: "Full power draw", units: "W", value: i => _.get(i, "powerConnection.PowerDraw") }),
    new ComparisonEntry({ title: "Power to EM ratio", units: "J/W", decimals: 1, value: i => _.get(i, "powerConnection.PowerToEM") }),
    new ComparisonEntry({ title: "EM at standby", units: "J", value: i => _.get(i, "powerConnection.PowerBase") * _.get(i, "powerConnection.PowerToEM") }),
    new ComparisonEntry({ title: "EM at full power", units: "J", value: i => _.get(i, "powerConnection.PowerDraw") * _.get(i, "powerConnection.PowerToEM") }),
    new ComparisonEntry({ title: "Shield", units: "HP", value: i => i.maxShieldHealth }),
  ]

  items: SCItem[] = [];

  private currentSortField: ComparisonEntry = this.fields[0];
  private currentSortDirection: "asc" | "desc" = "asc";

  constructor(private $http: HttpClient, private route: ActivatedRoute, private localisationSvc: LocalisationService) { }

  ngOnInit() {
    this.route.queryParamMap.subscribe(params => {

      let itemsParam: string = params.get("items") || "";
      let itemClasses = itemsParam.split(",");

      let itemPromises: Promise<SCItem>[] = [];

      this.items = [];
      for (let i = 0; i < itemClasses.length; i++) {
        itemPromises[i] = this.$http.get<any>(`${environment.api}/items/${itemClasses[i].toLowerCase()}.json`).toPromise().then(i => { this.items.push(new SCItem(i)); this.applySort(); return i; });
      }

      Promise.all(itemPromises).then((items: SCItem[]) => {
        //this.sortBy(this.currentSortField);
      });

    });
  }

  sortBy(field: ComparisonEntry) {
    if (field === this.currentSortField) this.currentSortDirection = this.currentSortDirection == "asc" ? "desc" : "asc";
    else this.currentSortDirection = field.sortDirection;
    this.currentSortField = field;
    this.applySort();
  }

  applySort() {
    this.items = _.orderBy(this.items, [i => this.currentSortField.value(i) || 0], [this.currentSortDirection]);
  }

}
