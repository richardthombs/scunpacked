import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from 'src/environments/environment';

import { LocalisationService } from './localisation.service';
import { ItemIndexEntry } from './ItemIndexEntry';

@Injectable()
export class ItemsResolver implements Resolve<ItemIndexEntry> {

  constructor(private $http: HttpClient, private localisationSvc: LocalisationService) { }

  resolve(): Observable<any> {

    return this.$http.get<ItemIndexEntry[]>(`${environment.api}/items.json`).pipe(map(r => {

      // Filter out anything that isn't a ship component
      r = r.filter(x => x.type);
      r = r.filter(x => !x.type.includes("AIModule"));
      r = r.filter(x => !x.type.includes("Battery"));
      r = r.filter(x => !x.type.includes("Button"));
      r = r.filter(x => !x.type.includes("Cargo"));
      r = r.filter(x => !x.type.includes("Controller"));
      r = r.filter(x => !x.type.includes("ControlPanel"));
      r = r.filter(x => !x.type.includes("Container"));
      r = r.filter(x => !x.type.includes("Display"));
      r = r.filter(x => !x.type.includes("Door"));
      r = r.filter(x => !x.type.includes("FuelIntake"));
      r = r.filter(x => !x.type.includes("FuelTank"));
      r = r.filter(x => !x.type.includes("LandingSystem"));
      r = r.filter(x => !x.type.includes("Lightgroup"));
      r = r.filter(x => !x.type.includes("MainThruster"));
      r = r.filter(x => !x.type.includes("ManneuverThruster"));
      r = r.filter(x => !x.type.includes("MiningArm"));
      r = r.filter(x => !x.type.includes("MiningModifier"));
      r = r.filter(x => !x.type.includes("Misc"));
      r = r.filter(x => !x.type.includes("MissileLauncher"));
      r = r.filter(x => !x.type.includes("Paints"));
      r = r.filter(x => !x.type.includes("Ping"));
      r = r.filter(x => !x.type.includes("Player"));
      r = r.filter(x => !x.type.includes("QuantumFuelTank"));
      r = r.filter(x => !x.type.includes("Seat"));
      r = r.filter(x => !x.type.includes("SeatAccess"));
      r = r.filter(x => !x.type.includes("SeatDashboard"));
      r = r.filter(x => !x.type.includes("SelfDestruct"));
      r = r.filter(x => !x.type.includes("Sensor"));
      r = r.filter(x => !x.type.includes("TargetSelector"));
      r = r.filter(x => !x.type.includes("Turret"));
      r = r.filter(x => !x.type.includes("TurretBase"));
      r = r.filter(x => !x.type.includes("UNDEFINED"));
      r = r.filter(x => !x.type.includes("Usable"));
      r = r.filter(x => !x.type.includes("WeaponAttachment"));

      return r;
    }));

  }
}
