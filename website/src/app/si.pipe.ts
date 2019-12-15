import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'si'
})
export class SiPipe implements PipeTransform {

  transform(value: number, units: string | undefined): any {
    var si = SiPipe.siPrefix(value);
    return `${Math.round(si.value)} ${si.prefix}${units || ""}`;
  }

  static siPrefix(value: number): { value: number, prefix: string } {

    if (isNaN(value)) return { value: 0, prefix: "" };
    if (!isFinite(value)) return { value: value, prefix: "" };

    if (value >= 1e24) return { value: (value / 1e24), prefix: "Y" };
    if (value >= 1e21) return { value: (value / 1e21), prefix: "Z" };
    if (value >= 1e18) return { value: (value / 1e18), prefix: "E" };
    if (value >= 1e15) return { value: (value / 1e15), prefix: "P" };
    if (value >= 1e12) return { value: (value / 1e12), prefix: "T" };
    if (value >= 1e9) return { value: (value / 1e9), prefix: "G" };
    if (value >= 1e6) return { value: (value / 1e6), prefix: "M" };
    if (value >= 1e3) return { value: (value / 1e3), prefix: "k" };
    if (value == 0) return { value: value, prefix: "" };

    if (value < 1e-6) return { value: value / 1e-9, prefix: "n" };
    if (value < 1e-3) return { value: value / 1e-6, prefix: "u" };
    if (value < 1) return { value: value / 1e-3, prefix: "m" };

    return { value: value, prefix: "" };
  }
}
