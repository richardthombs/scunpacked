import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import * as _ from 'lodash';

import { environment } from 'src/environments/environment';

import { SCItem } from '../SCItem';
import { LocalisationService } from '../localisation.service';
import { SiPipe } from '../si.pipe';

type FieldValue = number | string | undefined;
type FieldScore = { value: number, score: number, best: number, worst: number };

class ComparisonField {

  title: string = "";
  units: string = "";
  siPrefix: boolean = false;
  sortDirection: "asc" | "desc" = "asc";
  decimals: number = 0;
  valueFn: (item: SCItem) => FieldValue = () => undefined;
  formatFn: (value: FieldValue) => string | null = this.defaultFormat;
  linkFn?: (item: SCItem) => string = undefined;
  compareFn?: (all: SCItem[], item: SCItem) => FieldScore | undefined = this.compareValue;
  bestValue?: number;
  worstValue?: number;
  hidden: boolean = false;

  constructor(init?: Partial<ComparisonField>) {
    Object.assign(this, init);
  }

  formattedValue(item: SCItem): string {
    let v = this.valueFn(item);
    return this.formatFn(v) || "";
  }

  formattedCompareValue(all: SCItem[], b: SCItem): string {
    let diff = this.compareValue(all, b);
    if (diff === undefined) return "";

    if (diff.score === 1) return "";

    let x: number = diff.value - diff.best;

    let fmt = this.formatFn(x) || "";
    return x > 0 ? "+" + fmt : fmt;
  }

  compareValue(all: SCItem[], item: SCItem): FieldScore | undefined {
    if (!this.bestValue || !this.worstValue) return undefined;

    let itemValue = this.valueFn(item);
    if (!this.isNumber(itemValue)) return undefined;

    let score = 0;
    if (this.bestValue === this.worstValue) score = 1;
    else score = (itemValue - this.worstValue) / (this.bestValue - this.worstValue);

    return { value: itemValue, score: score, best: this.bestValue, worst: this.worstValue };
  }

  compareClass(all: SCItem[], b: SCItem): string {
    if (!this.compareFn) return "";

    let diff = this.compareValue(all, b);
    if (diff === undefined) return "";

    if (diff.score === 0) return "worst";
    if (diff.score === 1) return "best";
    if (diff.score >= 0.90) return "best-90"
    if (diff.score >= 0.80) return "best-80"
    if (diff.score >= 0.70) return "best-70"
    if (diff.score >= 0.60) return "best-60"
    if (diff.score >= 0.50) return "best-50"
    if (diff.score >= 0.40) return "best-40"
    if (diff.score >= 0.30) return "best-30"
    if (diff.score >= 0.20) return "best-20"
    if (diff.score >= 0.10) return "best-10"
    if (diff.score >= 0.00) return "best-00"

    return "";
  }

  private defaultFormat(v: FieldValue) {
    if (v === undefined) return "";

    let value = v, units = this.units, formatted = v.toString();

    if (this.isNumber(value)) {
      if (isFinite(value)) {
        if (this.siPrefix) {
          let si = SiPipe.siPrefix(value);
          value = si.value;
          units = si.prefix + this.units;
        }
        formatted = Number(value.toFixed(this.decimals)).toLocaleString();
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

type ComparisonGroup = {
  title: string;
  fields: ComparisonField[];
  visibleFn: (items: SCItem[]) => boolean;
}

@Component({
  selector: 'app-compare-items-page',
  templateUrl: './compare-items-page.component.html',
  styleUrls: ['./compare-items-page.component.scss']
})
export class CompareItemsPage implements OnInit {

  fields: ComparisonGroup[] = [
    {
      title: "",
      visibleFn: () => true,
      fields: [
        new ComparisonField({ title: "Name", valueFn: i => this.localisationSvc.getText(i.name, i.className), linkFn: i => `/items/${i.className.toLowerCase()}` }),
        new ComparisonField({ title: "Size", valueFn: i => i.size, formatFn: v => `Size ${v}`, compareFn: undefined }),
      ],
    },
    {
      title: "Quantum speed",
      visibleFn: items => !!_.find(items, i => i.quantumDrive),
      fields: [
        new ComparisonField({ title: "PO to ArcCorp (time)", units: "s", valueFn: i => i.secondsToArcCorp, formatFn: v => typeof v === "number" ? `${Math.floor(v / 60)}m ${Math.round(v % 60)}s` : "?" }),
        new ComparisonField({ title: "Quantum speed", units: "m/s", siPrefix: true, valueFn: i => i.driveSpeed, sortDirection: "desc" }),
        new ComparisonField({ title: "P1 acceleration", units: "m/s", valueFn: i => _.get(i, "quantumDrive.params.stageOneAccelRate"), sortDirection: "desc" }),
        new ComparisonField({ title: "P2 acceleration", units: "m/s", siPrefix: true, decimals: 1, valueFn: i => _.get(i, "quantumDrive.params.stageTwoAccelRate"), sortDirection: "desc" }),
        new ComparisonField({ title: "Engage speed", units: "m/s", siPrefix: true, decimals: 1, valueFn: i => _.get(i, "quantumDrive.params.engageSpeed") }),
      ]
    },
    {
      title: "Quantum efficiency",
      visibleFn: items => !!_.find(items, i => i.quantumDrive),
      fields: [
        new ComparisonField({ title: "PO to ArcCorp (fuel)", units: "l", valueFn: i => i.fuelToArcCorp }),
        new ComparisonField({ title: "Efficiency", units: "m/l", siPrefix: true, valueFn: i => 1 / i.quantumFuelRequirement, sortDirection: "desc" }),
        new ComparisonField({ title: "Quantum fuel requirement", units: "l/Gm", valueFn: i => i.quantumFuelRequirement * 1e9 }),
      ]
    },
    {
      title: "Power usage",
      visibleFn: items => !!_.find(items, i => i.powerConnection),
      fields: [
        new ComparisonField({ title: "Standby power draw", units: "W", valueFn: i => _.get(i, "powerConnection.PowerBase") }),
        new ComparisonField({ title: "Full power draw", units: "W", valueFn: i => _.get(i, "powerConnection.PowerDraw") }),
      ]
    },
    {
      title: "Power emmisions",
      visibleFn: items => !!_.find(items, i => i.powerConnection),
      fields: [
        new ComparisonField({ title: "Power to EM ratio", units: "J/W", decimals: 1, valueFn: i => _.get(i, "powerConnection.PowerToEM") }),
        new ComparisonField({ title: "EM at standby", units: "J", valueFn: i => _.get(i, "powerConnection.PowerBase") * _.get(i, "powerConnection.PowerToEM") }),
        new ComparisonField({ title: "EM at full power", units: "J", valueFn: i => _.get(i, "powerConnection.PowerDraw") * _.get(i, "powerConnection.PowerToEM") }),
      ]
    },
    {
      title: "Durability",
      visibleFn: items => !!_.find(items, i => !!i.degregation || !!i.health),
      fields: [
        new ComparisonField({ title: "Hitpoints", units: "HP", valueFn: i => i.health, sortDirection: "desc" }),
        new ComparisonField({ title: "Lifetime", units: "h", decimals: 1, valueFn: i => i.maxLifetime, sortDirection: "desc" }),
      ]
    },
    {
      title: "Shields",
      visibleFn: items => !!_.find(items, i => !!i.maxShieldHealth),
      fields: [
        new ComparisonField({ title: "Shield", units: "HP", valueFn: i => i.maxShieldHealth, sortDirection: "desc" }),
      ]
    }
  ];

  items: SCItem[] = [];

  private currentSortField: ComparisonField = this.fields[0].fields[0];
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

      Promise.all(itemPromises).then(() => {
        this.fields.forEach(g => {
          g.fields.forEach(f => {
            this.items.forEach(i => {
              let value = f.valueFn(i);
              if (value === undefined || typeof value !== "number") return;
              if (f.sortDirection == "asc" && (f.bestValue === undefined || value < f.bestValue)) f.bestValue = value;
              if (f.sortDirection == "asc" && (f.worstValue === undefined || value > f.worstValue)) f.worstValue = value;
              if (f.sortDirection == "desc" && (f.bestValue === undefined || value > f.bestValue)) f.bestValue = value;
              if (f.sortDirection == "desc" && (f.worstValue === undefined || value < f.worstValue)) f.worstValue = value;
            });
          });
        });
      });

    });
  }

  sortBy(field: ComparisonField) {
    if (field === this.currentSortField) this.currentSortDirection = this.currentSortDirection == "asc" ? "desc" : "asc";
    else this.currentSortDirection = field.sortDirection;
    this.currentSortField = field;
    this.applySort();
  }

  private applySort() {
    this.items = _.orderBy(this.items, [i => this.currentSortField.valueFn(i) || 0], [this.currentSortDirection]);
  }

}
