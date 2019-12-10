import * as _ from 'lodash';

import { ItemPortLoadout } from "./ItemPortLoadout";
import { Part } from './Part';
import { IItemPort } from "./IItemPort";

const distanceBetweenPOandArcCorp = 41927351070;

export class Ship {
  Loadout: ItemPortLoadout[] = [];
  Raw: any;
  Parts: Part[] = [];

  constructor(loadout: ItemPortLoadout[], raw: any) {
    this.Loadout = loadout;
    this.Raw = raw;
    if (this.Raw.Vehicle.Parts) this.Parts = _.map(this.Raw.Vehicle.Parts, x => new Part(x));
  }

  get className(): string {
    return _.get(this.Raw, "Entity.ClassName", "Unknown");
  }

  get scu(): number {
    return _.reduce(this.Loadout, (total, itemPort) => total + _.get(itemPort.loadedItem, "scu", 0), 0);
  }

  get maxShieldHealth(): number {
    return _.reduce(this.Loadout, (total, itemPort) => total + _.get(itemPort.loadedItem, "maxShieldHealth", 0), 0);
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

  get quantumFuelCapacity(): number {
    return _.reduce(this.Loadout, (total, itemPort) => {
      if (_.get(itemPort.loadedItem, "type", "") == "QuantumFuelTank") return total + _.get(itemPort.loadedItem, "capacity", 0);
      return total;
    }, 0);
  }

  get quantumFuelRequirement(): number {
    return _.reduce(this.Loadout, (total, itemPort) => {
      if (_.get(itemPort.loadedItem, "type", "") == "QuantumDrive") return total + _.get(itemPort.loadedItem, "quantumFuelRequirement", 0);
      return total;
    }, 0);
  }

  get quantumRange(): number {
    return this.quantumFuelCapacity / this.quantumFuelRequirement;
  }

  get quantumSpeed(): number {
    let drive = _.find(this.Loadout, x => _.get(x.loadedItem, "type", "") == "QuantumDrive");
    if (!drive || !drive.loadedItem) return 0;
    return drive.loadedItem.driveSpeed;
  }

  get secondsToArcCorp(): number {
    let speed = this.quantumSpeed;
    return distanceBetweenPOandArcCorp / speed;
  }

  get timesToArcCorpAndBack(): number {
    return this.quantumRange / (2 * distanceBetweenPOandArcCorp)
  }

  get fuelIntakePushRate(): number {
    return _.reduce(this.Loadout, (total, itemPort) => {
      if (_.get(itemPort.loadedItem, "type", "") == "FuelIntake") return total + _.get(itemPort.loadedItem, "fuelPushRate", 0);
      return total;
    }, 0);
  }

  get mainThrusterBurnRate(): number {
    return _.reduce(this.Loadout, (total, itemPort) => {
      if (_.get(itemPort.loadedItem, "type", "") == "MainThruster") return total + _.get(itemPort.loadedItem, "maxThrustFuelBurnRate", 0);
      return total;
    }, 0);
  }

  get manneuverThrusterBurnRate(): number {
    return _.reduce(this.Loadout, (total, itemPort) => {
      if (_.get(itemPort.loadedItem, "type", "") == "ManneuverThruster") return total + _.get(itemPort.loadedItem, "maxThrustFuelBurnRate", 0);
      return total;
    }, 0);
  }

  findParts(predicate?: (p: Part) => boolean): Part[] {
    var found: Part[] = [];
    this.Parts.forEach(part => {
      var s = part.findParts(predicate);
      found = found.concat(s);
    });
    return found;
  }

  findItemPorts(predicate?: (ip: IItemPort) => boolean): IItemPort[] {
    let found: IItemPort[] = [];

    let parts = this.findParts(p => p.class == "ItemPort" && !!p.itemPort);
    parts.forEach(p => {
      if (p.itemPort == null) return;
      found = found.concat(p.itemPort.findItemPorts(predicate));
    });

    return found;
  }
}
