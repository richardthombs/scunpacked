import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from 'src/environments/environment';

import { LocalisationService } from './localisation.service';

@Injectable()
export class LabelsResolver implements Resolve<any> {
  constructor(private $http: HttpClient) { }
  resolve(): Observable<any> | Promise<any> | any {
    return this.$http.get<{
      [id: string]: string;
    }>(`${environment.api}/labels.json`).toPromise().then(r => LocalisationService.SetLabels(r));
  }
}
