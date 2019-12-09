import { Pipe, PipeTransform } from '@angular/core';
import { DistancePipe } from './distance.pipe';

@Pipe({
  name: 'speed'
})
export class SpeedPipe implements PipeTransform {

  distancePipe: DistancePipe = new DistancePipe();

  transform(value: any, ...args: any[]): any {
    return this.distancePipe.transform(value) + "/s";
  }

}
