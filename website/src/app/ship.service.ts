import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as _ from 'lodash';
import { environment } from 'src/environments/environment';

import { Ship as StandardisedShip, StandardisedItemPort, StandardisedPortSummary } from './Ship';

@Injectable({
  providedIn: 'root'
})
export class ShipService {

  private itemCache: { [id: string]: any } = {}

  constructor(private $http: HttpClient) { }

  async load(shipClass: string): Promise<LoadedShip> {
    let shipObj = await this.$http.get<StandardisedShip>(`${environment.api}/v2/ships/${shipClass.toLowerCase()}.json`).toPromise();
    let portsObj = await this.$http.get<StandardisedPortSummary>(`${environment.api}/v2/ships/${shipClass.toLowerCase()}-ports.json`).toPromise();

    console.log("Loaded ship", shipClass, shipObj, portsObj);

    return {
      Ship: shipObj,
      Ports: portsObj
    };
  }

}

export interface LoadedShip {
  Ship: StandardisedShip;
  Ports: StandardisedPortSummary;
}

export interface StandardisedPart {
  Name: string;
  Port: StandardisedItemPort;
  Parts: StandardisedPart[];
}
