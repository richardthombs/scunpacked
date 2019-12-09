import { Pipe, PipeTransform } from '@angular/core';

import { SiPipe } from './si.pipe';

const parsec: number = 30856775814913673;

@Pipe({
  name: 'distance'
})
export class DistancePipe implements PipeTransform {

  transform(value: number, ...args: any[]): any {
    let unit = "m";
    if (value >= 1e30) {
      value = value / parsec;
      unit = "pc";
    }
    var si = SiPipe.siPrefix(value);
    return `${Math.round(si.value)} ${si.prefix}${unit}`;
  }

}
