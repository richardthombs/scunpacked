import { SCItem } from "./SCItem";
import { SiPipe } from './si.pipe';

export type FieldValue = number | string | undefined;
export type FieldScore = { value: number, diff: number, score: number, best: number, worst: number };

export class ComparisonGroup<T> {
  title: string = "";
  fields: ComparisonField<T>[] = [];
  visibleFn: (items: T[]) => boolean = () => true;
  collapsed: boolean = false;

  constructor(init?: Partial<ComparisonGroup<T>>) {
    Object.assign(this, init);
  }
}

export class ComparisonField<T> {

  title: string = "";
  units: string = "";
  siPrefix: boolean = false;
  sortDirection: "asc" | "desc" = "asc";
  decimals: number = 0;
  valueFn: (item: T) => FieldValue = () => undefined;
  formatFn: (value: FieldValue) => string | null = this.defaultFormat;
  linkFn?: (item: T) => string = undefined;
  compareFn?: (all: T[], item: T) => FieldScore | undefined = this.compareValue;
  bestValue?: number;
  worstValue?: number;
  hidden: boolean = false;
  showPlus: boolean = false;

  constructor(init?: Partial<ComparisonField<T>>) {
    Object.assign(this, init);
  }

  formattedValue(item: T): string {
    let v = this.valueFn(item);
    if (v === undefined) return "";

    let fmt = this.formatFn(v) || "";
    return v >= 0 && this.showPlus ? "+" + fmt : fmt;
  }

  formattedCompareValue(all: T[], b: T): string {
    let cmp = this.compareValue(all, b);
    if (cmp === undefined) return "";

    if (cmp.diff === 0) return "";

    let x: number = cmp.diff;

    let fmt = this.formatFn(x) || "";
    return x > 0 ? "+" + fmt : fmt;
  }

  compareValue(all: T[], item: T): FieldScore | undefined {
    return this.compareAgainstFirst(all[0], item);
  }

  private compareAgainstFirst(first: T, item: T) {
    let cmp = this.compareAgainstAll(item);
    if (!cmp) return undefined;

    let firstValue = this.valueFn(first);
    if (this.isNumber(firstValue)) {
      cmp.diff = cmp.value - firstValue;
    }

    return cmp;
  }

  private compareAgainstAll(item: T): FieldScore | undefined {
    if (this.bestValue === undefined || this.worstValue === undefined) return undefined;

    let itemValue = this.valueFn(item);
    if (!this.isNumber(itemValue)) return undefined;

    let score = 0;
    if (this.bestValue === this.worstValue) score = 1;
    else score = (itemValue - this.worstValue) / (this.bestValue - this.worstValue);

    return { value: itemValue, diff: itemValue - this.bestValue, score: score, best: this.bestValue, worst: this.worstValue };
  }

  compareClass(all: T[], b: T): string {
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

  differenceClass(all: T[], b: T): string {
    if (!this.compareFn) return "";

    let cmp = this.compareValue(all, b);
    if (cmp === undefined) return "";


    if (this.sortDirection == "asc") {
      if (cmp.diff > 0) return "worse";
      if (cmp.diff < 0) return "better";
    }
    else {
      if (cmp.diff < 0) return "worse";
      if (cmp.diff > 0) return "better";
    }

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
