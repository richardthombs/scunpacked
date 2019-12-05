import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'nozero'
})
export class NoZeroPipe implements PipeTransform {

  transform(value: number, ...args: any[]): any {
    if (value) return value;
    return "";
  }

}
