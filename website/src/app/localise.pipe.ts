import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'localise'
})
export class LocalisePipe implements PipeTransform {

  static strings: { [id: string]: string }

  transform(value: string, ...args: any[]): string {
    if (!value) return "";

    var label = value.substring(1);
    var text = LocalisePipe.strings && LocalisePipe.strings[label] ? LocalisePipe.strings[label] : value;
    return text.replace(/\\n/g, "\n");
  }

  static SetLabels(labels: any) {
    this.strings = labels;
  }

}
