import * as _ from 'lodash';

import { Item } from './Item';

export class SCItem {

  constructor(private item: Item) {
  }

  get name(): string {
    return _.get(this.item, "Raw.Entity.Components.SAttachableComponentParams.AttachDef.Localization.Name");
  }

  get type(): string {
    return _.get(this.item, "Raw.Entity.Components.SAttachableComponentParams.AttachDef.Type");
  }

  get subType(): string {
    return _.get(this.item, "Raw.Entity.Components.SAttachableComponentParams.AttachDef.SubType");
  }

  get powerDraw(): number {
    return _.get(this.item, "Raw.Entity.Components.EntityComponentPowerConnection.PowerDraw", 0);
  }

  get scu(): number {
    let size = _.get(this.item, "Raw.Entity.Components.SCItemCargoGridParams.dimensions", { x: 0, y: 0, z: 0 });
    return Math.floor(size.x / 1.25) * Math.floor(size.y / 1.25) * Math.floor(size.z / 1.25);
  }

  get maxShieldHealth(): number {
    return _.get(this.item, "Raw.Entity.Components.SCItemShieldGeneratorParams.MaxShieldHealth", 0);
  }

  get capacity(): number {
    return _.get(this.item, "Raw.Entity.Components.SCItemFuelTankParams.capacity", 0);
  }

  get quantumFuelRequirement(): number {
    // Believe this value is in fuel / Mm, so divide by 1000 to turn it into fuel / m
    return _.get(this.item, "Raw.Entity.Components.SCItemQuantumDriveParams.quantumFuelRequirement", 0) / 1e6;
  }

  get jumpRange(): number {
    return _.get(this.item, "Raw.Entity.Components.SCItemQuantumDriveParams.jumpRange", 0);
  }

  get driveSpeed(): number {
    return _.get(this.item, "Raw.Entity.Components.SCItemQuantumDriveParams.params.driveSpeed", 0);
  }

  get thrustCapacity(): number {
    return _.get(this.item, "Raw.Entity.Components.SCItemThrusterParams.thrustCapacity", 0);
  }

  get fuelBurnRate(): number {
    // Rate is per 10KN, so divide by 1e4 to get the rate per newton
    return _.get(this.item, "Raw.Entity.Components.SCItemThrusterParams.fuelBurnRatePer10KNewton", 0) / 1e4;
  }

  get fuelPushRate(): number {
    return _.get(this.item, "Raw.Entity.Components.SCItemFuelIntakeParams.fuelPushRate", 0);
  }

  get maxThrustFuelBurnRate(): number {
    return this.thrustCapacity * this.fuelBurnRate;
  }

  get Raw(): any {
    return this.item.Raw;
  }
}
