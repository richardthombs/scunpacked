import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../environments/environment';

import { LocalisationService } from './Localisation';
import { ItemIndexEntry } from './ItemIndexEntry';

@Injectable()
export class FpsItemsResolver implements Resolve<ItemIndexEntry> {

  constructor(private $http: HttpClient, private localisationSvc: LocalisationService) { }

  resolve(): Observable<any> {

    return this.$http.get<ItemIndexEntry[]>(`${environment.api}/fps-items.json`).pipe(map(r => {

      return r;

    }));

  }
}
