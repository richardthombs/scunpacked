import * as _ from 'lodash';

import { ItemPort } from "./ItemPort";

export class Ship {
  Loadout: ItemPort[] = [];
  Raw: any;

  constructor(loadout: ItemPort[], raw: any) {
    this.Loadout = loadout;
    this.Raw = raw;
  }

  get scu(): number {
    return _.reduce(this.Loadout, (total, itemPort) => total += _.get(itemPort.loadedItem, "scu", 0), 0);
  }

  get maxShieldHealth(): number {
    return _.reduce(this.Loadout, (total, itemPort) => total += _.get(itemPort.loadedItem, "maxShieldHealth", 0), 0);
  }

  get vehicleName(): string {
    return _.get(this.Raw, "Entity.Components.VehicleComponentParams.vehicleName", "Unknown");
  }

  get vehicleDescription(): string {
    return _.get(this.Raw, "Entity.Components.VehicleComponentParams.vehicleDescription", "Unknown");
  }

  get vehicleCareer(): string {
    return _.get(this.Raw, "Entity.Components.VehicleComponentParams.vehicleCareer", "Unknown career");
  }

  get vehicleRole(): string {
    return _.get(this.Raw, "Entity.Components.VehicleComponentParams.vehicleRole", "Unknown role");
  }

  get vehicleSize(): string {
    return _.get(this.Raw, "Entity.Components.SAttachableComponentParams.AttachDef.Size", "Unknown");
  }

  get crewSize(): number {
    return _.get(this.Raw, "Entity.Components.VehicleComponentParams.crewSize", 0);
  }
}
