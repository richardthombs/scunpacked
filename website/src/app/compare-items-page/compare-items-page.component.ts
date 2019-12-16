import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import * as _ from 'lodash';

import { environment } from 'src/environments/environment';

import { SCItem } from '../SCItem';
import { LocalisationService } from '../localisation.service';
import { SiPipe } from '../si.pipe';

type FieldValue = number | string | undefined;

class ComparisonField {

  title: string = "";
  units: string = "";
  siPrefix: boolean = false;
  sortDirection: "asc" | "desc" = "asc";
  decimals: number = 0;
  valueFn: (item: SCItem) => FieldValue = () => undefined;
  formatFn: (value: FieldValue) => string | null = this.defaultFormat;
  linkFn?: (item: SCItem) => string = undefined;
  compareFn?: (a: SCItem, b: SCItem) => number | undefined = this.compare

  constructor(init?: Partial<ComparisonField>) {
    Object.assign(this, init);
  }

  formattedValue(item: SCItem): string {
    let v = this.valueFn(item);
    return this.formatFn(v) || "";
  }

  formattedCompare(a: SCItem, b: SCItem): string {
    let diff = this.compare(a, b);
    if (!diff) return "";

    var formatted = this.formatFn(diff);
    if (formatted) return diff > 0 ? "+" + formatted : formatted;

    return "";
  }

  compare(a: SCItem, b: SCItem): number | undefined {
    let va = this.valueFn(a), vb = this.valueFn(b);
    if (!this.isNumber(va) || !this.isNumber(vb)) return undefined;

    return vb - va;
  }

  compareClass(a: SCItem, b: SCItem): string {
    let diff = this.compare(a, b);
    if (!diff) return "same";

    if (diff > 0) return this.sortDirection == "desc" ? "better" : "worse";
    if (diff < 0) return this.sortDirection == "asc" ? "better" : "worse";

    return "";
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

  fields: ComparisonField[] = [
    new ComparisonField({ title: "Name", valueFn: i => this.localisationSvc.getText(i.name, i.className), linkFn: i => `/items/${i.className.toLowerCase()}` }),
    new ComparisonField({ title: "Size", valueFn: i => i.size, formatFn: v => `Size ${v}`, compareFn: undefined }),
    new ComparisonField({ title: "PO to ArcCorp (time)", units: "s", valueFn: i => i.secondsToArcCorp, formatFn: v => typeof v === "number" ? `${Math.floor(v / 60)}m ${Math.round(v % 60)}s` : "?" }),
    new ComparisonField({ title: "PO to ArcCorp (fuel)", units: "l", valueFn: i => i.fuelToArcCorp }),
    new ComparisonField({ title: "Efficiency", units: "m/l", siPrefix: true, valueFn: i => 1 / i.quantumFuelRequirement, sortDirection: "desc" }),
    new ComparisonField({ title: "Quantum speed", units: "m/s", siPrefix: true, valueFn: i => i.driveSpeed }),
    new ComparisonField({ title: "Quantum fuel requirement", units: "l/Gm", valueFn: i => i.quantumFuelRequirement * 1e9 }),
    new ComparisonField({ title: "P1 acceleration", units: "m/s", valueFn: i => _.get(i, "quantumDrive.params.stageOneAccelRate"), sortDirection: "desc" }),
    new ComparisonField({ title: "P1 acceleration", units: "m/s", siPrefix: true, decimals: 1, valueFn: i => _.get(i, "quantumDrive.params.stageTwoAccelRate"), sortDirection: "desc" }),
    new ComparisonField({ title: "Engage speed", units: "m/s", siPrefix: true, decimals: 1, valueFn: i => _.get(i, "quantumDrive.params.engageSpeed") }),
    new ComparisonField({ title: "Standby power draw", units: "W", valueFn: i => _.get(i, "powerConnection.PowerBase") }),
    new ComparisonField({ title: "Full power draw", units: "W", valueFn: i => _.get(i, "powerConnection.PowerDraw") }),
    new ComparisonField({ title: "Power to EM ratio", units: "J/W", decimals: 1, valueFn: i => _.get(i, "powerConnection.PowerToEM") }),
    new ComparisonField({ title: "EM at standby", units: "J", valueFn: i => _.get(i, "powerConnection.PowerBase") * _.get(i, "powerConnection.PowerToEM") }),
    new ComparisonField({ title: "EM at full power", units: "J", valueFn: i => _.get(i, "powerConnection.PowerDraw") * _.get(i, "powerConnection.PowerToEM") }),
    new ComparisonField({ title: "Lifetime", units: "h", decimals: 1, valueFn: i => i.maxLifetime, sortDirection: "desc" }),
    new ComparisonField({ title: "Shield", units: "HP", valueFn: i => i.maxShieldHealth }),
  ]

  items: SCItem[] = [];

  private currentSortField: ComparisonField = this.fields[0];
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

  sortBy(field: ComparisonField) {
    if (field === this.currentSortField) this.currentSortDirection = this.currentSortDirection == "asc" ? "desc" : "asc";
    else this.currentSortDirection = field.sortDirection;
    this.currentSortField = field;
    this.applySort();
  }

  applySort() {
    this.items = _.orderBy(this.items, [i => this.currentSortField.valueFn(i) || 0], [this.currentSortDirection]);
  }

}
