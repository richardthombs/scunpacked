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

    return this.$http.get<ShipIndexEntry[]>(`${environment.api}/v2/ships.json`).pipe(map(r => {
      // Fix ships without names
      //r.forEach(s => s.Name = (s.Name == "<= PLACEHOLDER =>" || s.Name == "<= UNINITIALIZED =>") ? s.ClassName : s.Name);
      // Figure out roles and sub-roles
      r.forEach(s => {
        if (s.Career == "@LOC_PLACEHOLDER")
          s.roles = [{ role: "Under development", subRole: "General" }];
        else {
          s.roles = [{ role: s.Career || "Under development", subRole: s.Role || "General" }];
          // Add a by size role and sub-role
          if (s.IsSpaceship)
            s.roles.push({ role: "By size", subRole: `Size ${s.Size || 0}` });

          // Add a manufacturer role
          s.roles.push({ role: "By manufacturer", subRole: s.Manufacturer.Name })
        }
      });

      return r;

    }));

  }
}
