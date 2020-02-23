import { Pipe, PipeTransform } from '@angular/core';

import { LocalisationService } from './Localisation';

@Pipe({
  name: 'localise'
})
export class LocalisePipe implements PipeTransform {

  constructor(private localisationSvc: LocalisationService) { }

  static strings: { [id: string]: string }

  transform(value: string, ...args: any[]): string {
    let text = this.localisationSvc.getText(value, args[0]);
    return text;
  }
}
