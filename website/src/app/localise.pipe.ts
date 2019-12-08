import { Pipe, PipeTransform } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable, of } from "rxjs"

@Pipe({
  name: 'localise'
})
export class LocalisePipe implements PipeTransform {

  static strings: { [id: string]: string }

  constructor(private $http: HttpClient) { }

  transform(value: string, ...args: any[]): Observable<string> {
    if (!LocalisePipe.strings) {
      console.log("Load strings");
      LocalisePipe.strings = { "blah": "blah" };
    }

    if (LocalisePipe.strings && LocalisePipe.strings[value]) return of(LocalisePipe.strings[value]);
    else return of(value);
  }

}
