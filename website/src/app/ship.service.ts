import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as _ from 'lodash';

import { Ship } from './Ship';
import { environment } from 'src/environments/environment';
import { SCItem } from './SCItem';
import { ItemPort } from './ItemPort';
import { IItemPort } from './IItemPort';
import { JsonLoadout, SEntityComponentDefaultLoadoutParams, SItemPortLoadoutManualParams, SItemPortLoadoutXMLParams } from './JsonLoadout';

@Injectable({
  providedIn: 'root'
})
export class ShipService {

  private itemCache: { [id: string]: any } = {}

  constructor(private $http: HttpClient) { }

  async load(shipClass: string): Promise<Ship> {
    let shipObj = await this.$http.get<any>(`${environment.api}/ships/${shipClass.toLowerCase()}.json`).toPromise();
    console.log(shipObj);
    let ship = new Ship(shipObj.Raw);

    console.log("Initialising loadout");
    let vehiclePorts = ship.findItemPorts(ip => ip instanceof ItemPort);
    let loadout = await this.getLoadout(ship.Raw.Entity.Components.SEntityComponentDefaultLoadoutParams);
    if (vehiclePorts.length && loadout.length) await this.loadItems(vehiclePorts, loadout);
    console.log("Loadout initialised");

    return ship;
  }

  async loadItems(itemPorts: IItemPort[], loadouts: JsonLoadout[]): Promise<void> {
    for (let i = 0; i < itemPorts.length; i++) {
      let itemPort = itemPorts[i];
      let loadout: JsonLoadout | undefined = _.find(loadouts, x => x.portName == itemPort.name);
      if (loadout && loadout.itemName) {
        itemPort.itemClass = loadout.itemName;
        itemPort.item = await this.loadItem(loadout.itemName);
        if (itemPort.item) {
          let subPorts = itemPort.item.findItemPorts();
          let manualLoadout = await this.getLoadout(_.get(itemPort.item.Raw, "Entity.Components.SEntityComponentDefaultLoadoutParams", []));
          let combinedLoadout: JsonLoadout[] = (loadout.Items || []).concat(manualLoadout || []);
          if (subPorts.length && combinedLoadout.length) await this.loadItems(subPorts, combinedLoadout);
        }
      }
    }
  }

  async loadItem(itemName: string): Promise<SCItem | undefined> {
    let loaded: any;

    if (itemName) {
      if (this.itemCache[itemName]) {
        loaded = this.itemCache[itemName];
        console.log("Loaded cached item", itemName);
      }
      else {
        loaded = await this.$http.get<any>(`${environment.api}/items/${itemName.toLowerCase()}.json`).toPromise().catch(e => { });
        console.log(loaded ? "Loaded item" : "Could not load item", itemName);
        if (loaded) this.itemCache[itemName] = loaded;
      }
    }

    if (!loaded) return undefined;

    // Clone so that each itemPort gets a unique object
    return new SCItem(JSON.parse(JSON.stringify(loaded)));
  }

  private async getLoadout(defaultLoadoutParams: SEntityComponentDefaultLoadoutParams): Promise<JsonLoadout[]> {
    if (!defaultLoadoutParams || !defaultLoadoutParams.loadout) return [];

    let loadouts: JsonLoadout[] = [];
    if (defaultLoadoutParams.loadout.SItemPortLoadoutManualParams) loadouts = loadouts.concat(await this.getManualLoadout(defaultLoadoutParams.loadout.SItemPortLoadoutManualParams));
    if (defaultLoadoutParams.loadout.SItemPortLoadoutXMLParams) loadouts = loadouts.concat(await this.getXmlLoadout(defaultLoadoutParams.loadout.SItemPortLoadoutXMLParams));
    return loadouts;
  }

  private async getManualLoadout(params: SItemPortLoadoutManualParams): Promise<JsonLoadout[]> {
    if (!params || !params.entries) return [];

    let loadouts: JsonLoadout[] = [];

    for (let i = 0; i < params.entries.length; i++) {
      let entry = params.entries[i];
      let subEntries = await this.getLoadout(entry);
      let loadout = { itemName: entry.entityClassName, portName: entry.itemPortName, Items: subEntries };
      loadouts.push(loadout);
    }

    return loadouts;
  }

  private async getXmlLoadout(params: SItemPortLoadoutXMLParams): Promise<JsonLoadout[]> {
    let loadouts = await this.$http.get<any>(`${environment.api}/loadouts/${params.loadoutPath}`).toPromise().catch(e => { });

    if (loadouts) console.log("Loaded loadout", params.loadoutPath);
    else console.error("Could not load loadout", params.loadoutPath);

    if (loadouts) return loadouts.Items || [];
    else return [];
  }
}
