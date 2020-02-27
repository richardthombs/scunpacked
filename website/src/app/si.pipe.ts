import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'si'
})
export class SiPipe implements PipeTransform {

  transform(value: any, units?: string, decimals?: number): string {
    if (value === undefined || value === null) return "";

    var si = SiPipe.siPrefix(value);

    return `${si.value.toFixed(decimals || 0)} ${si.prefix}${units || ""}`;
  }

  static siPrefix(value: number): { value: number, prefix: string } {

    if (isNaN(value)) return { value: 0, prefix: "" };
    if (!isFinite(value)) return { value: value, prefix: "" };

    let abs = Math.abs(value);

    if (abs >= 1e24) return { value: (value / 1e24), prefix: "Y" };
    if (abs >= 1e21) return { value: (value / 1e21), prefix: "Z" };
    if (abs >= 1e18) return { value: (value / 1e18), prefix: "E" };
    if (abs >= 1e15) return { value: (value / 1e15), prefix: "P" };
    if (abs >= 1e12) return { value: (value / 1e12), prefix: "T" };
    if (abs >= 1e9) return { value: (value / 1e9), prefix: "G" };
    if (abs >= 1e6) return { value: (value / 1e6), prefix: "M" };
    if (abs >= 1e3) return { value: (value / 1e3), prefix: "K" };
    if (abs == 0) return { value: value, prefix: "" };

    if (abs < 1e-6) return { value: value / 1e-9, prefix: "n" };
    if (abs < 1e-3) return { value: value / 1e-6, prefix: "u" };
    if (abs < 1) return { value: value / 1e-3, prefix: "m" };

    return { value: value, prefix: "" };
  }
}
