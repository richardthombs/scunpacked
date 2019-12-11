import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'localise'
})
export class LocalisePipe implements PipeTransform {

  static strings: { [id: string]: string }

  transform(value: string, ...args: any[]): string {
    if (!value) return "";
    if (value == "@LOC_PLACEHOLDER") return args[0] || "";

    var label = value.startsWith("@") ? value.substring(1) : value;
    var text = LocalisePipe.strings && LocalisePipe.strings[label] ? LocalisePipe.strings[label] : args[0] || value;
    return text.replace(/\\n/g, "\n");
  }

  static SetLabels(labels: any) {
    this.strings = labels;
  }

}
