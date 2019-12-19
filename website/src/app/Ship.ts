import * as _ from 'lodash';

import { Part } from './Part';
import { IItemPort } from "./IItemPort";

const distanceBetweenPOandArcCorp = 41927351070;

export class Ship {
  Raw: any;
  Parts: Part[] = [];

  constructor(raw: any) {
    this.Raw = raw;
    if (this.Raw.Vehicle && this.Raw.Vehicle.Parts) this.Parts = _.map(this.Raw.Vehicle.Parts, x => new Part(x));
  }

  get className(): string {
    return _.get(this.Raw, "Entity.ClassName", "Unknown");
  }

  get scu(): number {
    return _.reduce(this.findItemPorts(ip => ip.types.includes("Cargo")), (total, ip) => total + _.get(ip.item, "scu", 0), 0);
  }

  get maxShieldHealth(): number {
    return _.reduce(this.findItemPorts(ip => ip.types.includes("Shield")), (total, ip) => total + _.get(ip.item, "maxShieldHealth", 0), 0);
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
    return _.reduce(this.findItemPorts(ip => ip.item != null && ip.item.type == "QuantumFuelTank"), (total, ip) => total + _.get(ip.item, "capacity", 0), 0);
  }

  get quantumFuelRequirement(): number {
    return _.reduce(this.findItemPorts(ip => ip.item != null && ip.item.type == "QuantumDrive"), (total, ip) => total + _.get(ip.item, "quantumFuelRequirement", 0), 0);
  }

  get quantumRange(): number {
    return this.quantumFuelCapacity / this.quantumFuelRequirement;
  }

  get quantumSpeed(): number {
    let drives = this.findItemPorts(ip => ip.item != null && ip.item.type == "QuantumDrive");
    if (drives.length == 0 || !drives[0].item) return 0;
    return drives[0].item.driveSpeed;
  }

  get secondsToArcCorp(): number {
    let speed = this.quantumSpeed;
    return distanceBetweenPOandArcCorp / speed;
  }
  get fuelToArcCorp(): number {
    return distanceBetweenPOandArcCorp * this.quantumFuelRequirement;
  }

  get timesToArcCorpAndBack(): number {
    return this.quantumRange / (2 * distanceBetweenPOandArcCorp)
  }

  get fuelIntakePushRate(): number {
    return _.reduce(this.findItemPorts(ip => ip.item != null && ip.item.type == "FuelIntake"), (total, ip) => total + _.get(ip.item, "fuelPushRate", 0), 0);
  }

  get mainThrusterBurnRate(): number {
    return _.reduce(this.findItemPorts(ip => ip.item != null && ip.item.type == "MainThruster"), (total, ip) => total + _.get(ip.item, "maxThrustFuelBurnRate", 0), 0);
  }

  get manneuverThrusterBurnRate(): number {
    return _.reduce(this.findItemPorts(ip => ip.item != null && ip.item.type == "ManneuverThruster"), (total, ip) => total + _.get(ip.item, "maxThrustFuelBurnRate", 0), 0);
  }

  get totalPowerBase(): number {
    return _.reduce(this.findItemPorts(ip => ip.item != null && ip.item.type != "PowerPlant"), (total, ip) => total + _.get(ip.item, "powerConnection.PowerBase", 0), 0);
  }

  get totalPowerDraw(): number {
    return _.reduce(this.findItemPorts(ip => ip.item != null && ip.item.type != "PowerPlant"), (total, ip) => total + _.get(ip.item, "powerConnection.PowerDraw", 0), 0);
  }

  get maxSpeed(): number {
    var ifcs = this.findItemPorts(ip => ip.item != null && ip.item.type == "FlightController");
    if (ifcs.length) return _.get(ifcs[0].item, "Raw.Entity.Components.IFCSParams.maxAfterburnSpeed", 0);
    else return 0;
  }

  get scmSpeed(): number {
    var ifcs = this.findItemPorts(ip => ip.item != null && ip.item.type == "FlightController");
    if (ifcs.length) return _.get(ifcs[0].item, "Raw.Entity.Components.IFCSParams.maxSpeed", 0);
    else return 0;
  }

  get hitPoints(): number {
    return _.reduce(this.findParts(), (total, part) => total + part.damageMax, 0);
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
