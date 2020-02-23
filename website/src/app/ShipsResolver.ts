import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from 'src/environments/environment';

import { LocalisationService } from './Localisation';
import { ShipIndexEntry } from './ShipIndexEntry';

@Injectable()
export class ShipsResolver implements Resolve<ShipIndexEntry> {

  constructor(private $http: HttpClient, private localisationSvc: LocalisationService) { }

  resolve(): Observable<any> {

    return this.$http.get<ShipIndexEntry[]>(`${environment.api}/ships.json`).pipe(map(r => {
      // Fix ships without names
      r.forEach(s => s.name = (s.name == "@LOC_PLACEHOLDER" || s.name == "@LOC_UNINITIALIZED") ? s.className : s.name);
      // Figure out roles and sub-roles
      r.forEach(s => {
        if (!s.dogFightEnabled || s.career == "@LOC_PLACEHOLDER" || s.noParts)
          s.roles = [{ role: "Under development", subRole: "General" }];
        else {
          s.roles = [{ role: this.localisationSvc.getText(s.career, "Under development"), subRole: this.localisationSvc.getText(s.role, "General") }];
          // Add a by size role and sub-role
          if (s.isSpaceship)
            s.roles.push({ role: "Ships by size", subRole: `Size ${s.size || 0}` });
        }
      });

      return r;

    }));

  }
}
