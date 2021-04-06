import * as _ from 'lodash';

import { SCItemItemPort } from "./SCItemItemPort";
import { IItemPort } from "./IItemPort";

const distanceBetweenPOandArcCorp = 41927351070;

export interface StandardisedItem {
  ClassName: string;
  Size: number;
  Grade: number;
  Type: string;
  Classification: string;
  Name: string;
  Description: string;
  Manufacturer: {
    Code: string;
    Name: string;
  };
  Tags: string[];
  Shield: {
    Health: number,
    Regeneration: number,
    DamagedDelay: number,
    DownedDelay: number,
    Absorption: {
      Physical: StandardisedShieldAbsorptionRange,
      Energy: StandardisedShieldAbsorptionRange,
      Distortion: StandardisedShieldAbsorptionRange,
      Thermal: StandardisedShieldAbsorptionRange,
      Biochemical: StandardisedShieldAbsorptionRange,
      Stun: StandardisedShieldAbsorptionRange
    }
  };
  QuantumDrive: {
    FuelRate: number,
    JumpRange: number,
    StandardJump: {
      Speed: number,
      Cooldown: number,
      SpoolUpTime: number
    }
  };
  PowerPlant: {
    Output: number;
  };
  Cooler: {
    Rate: number;
  };
  Thruster: {};
  Durability: {
    Health: number;
    Lifetime: number;
  };
  CargoGrid: {};
  HydrogenFuelTank: {};
  QuantumFuelTank: {};
  HydrogenFuelIntake: {};
  Armor: {};
  Emp: {
    ChargeTime: number;
    Damage: number;
    Radius: number;
    CooldownTime: number;
  };
  MissileRack: {};
  QuantumInterdiction: {
    JammingRange: number;
    InterdictionRange: number;
  };
  Ifcs: {};
  PowerConnection: {
    PowerBase: number;
    PowerDraw: number;
  };
  HeatConnection: {
    ThermalEnergyBase: number;
    ThermalEnergyDraw: number;
    CoolingRate: number;
  };
  Weapon: {
    Modes: {
      Name: string;
      LocalisedName: string;
      RoundsPerMinute: number;
      FireType: string;
      AmmoPerShot: number;
      PelletsPerShot: number;
      DamagePerShot: StandardisedDamage;
      DamagePerSecond: StandardisedDamage;
    }[];
  },
  Missile: {
    Damage: StandardisedDamage;
  },
  Radar: {
    DetectionLifetime: number,
    AltitudeCeiling: number,
    CrossSectionOcclusion: boolean,
    Signatures: {
      Detectable: boolean,
      Sensitivity: number,
      AmbientPiercing: boolean
    }[]
  },
  Ping: {
    ChargeTime: number,
    CooldownTime: number
  },
  Scanner: {
    Range: number
  },
  Armour: {
    DamageMultipliers: StandardisedDamage,
    SignalMultipliers: StandardisedSignature
  }
}

export interface StandardisedDamage {
  Physical: number;
  Energy: number;
  Distortion: number;
  Thermal: number;
  Biochemical: number;
  Stun: number;
}

export interface StandardisedSignature {
  CrossSection: number;
  Infrared: number;
  Electromagnetic: number;
}

export interface StandardisedShieldAbsorptionRange {
  Minimum: number;
  Maximum: number;
}

export class SCItem {

  private _itemPorts?: IItemPort[];

  parentPort?: IItemPort;

  constructor(private json: any) {
  }

  get name(): string {
    return _.get(this.json, "Raw.Entity.Components.SAttachableComponentParams.AttachDef.Localization.Name", "");
  }

  get description(): string {
    return _.get(this.json, "Raw.Entity.Components.SAttachableComponentParams.AttachDef.Localization.Description", "");
  }

  get className(): string {
    return _.get(this.json, "Raw.Entity.ClassName");
  }

  get type(): string {
    return _.get(this.json, "Raw.Entity.Components.SAttachableComponentParams.AttachDef.Type", "");
  }

  get subType(): string {
    let subType = _.get(this.json, "Raw.Entity.Components.SAttachableComponentParams.AttachDef.SubType", "");
    if (subType == "UNDEFINED") subType = "";
    return subType;
  }

  get size(): number {
    return _.get(this.json, "Raw.Entity.Components.SAttachableComponentParams.AttachDef.Size", 0);
  }

  get grade(): number {
    return _.get(this.json, "Raw.Entity.Components.SAttachableComponentParams.AttachDef.Grade", 0);
  }

  get powerConnection(): any {
    return _.get(this.json, "Raw.Entity.Components.EntityComponentPowerConnection");
  }

  get powerDraw(): number {
    return _.get(this.json, "Raw.Entity.Components.EntityComponentPowerConnection.PowerDraw", 0);
  }

  get scu(): number {
    let size = _.get(this.json, "Raw.Entity.Components.SCItemCargoGridParams.dimensions", { x: 0, y: 0, z: 0 });
    return Math.floor(size.x / 1.25) * Math.floor(size.y / 1.25) * Math.floor(size.z / 1.25);
  }

  get shieldGenerator(): any {
    return _.get(this.json, "Raw.Entity.Components.SCItemShieldGeneratorParams");
  }

  get maxShieldHealth(): number {
    return _.get(this.json, "Raw.Entity.Components.SCItemShieldGeneratorParams.MaxShieldHealth", 0);
  }

  get maxShieldRegen(): number {
    return _.get(this.json, "Raw.Entity.Components.SCItemShieldGeneratorParams.MaxShieldRegen", 0);
  }

  get capacity(): number {
    return _.get(this.json, "Raw.Entity.Components.SCItemFuelTankParams.capacity", 0);
  }

  get thrustCapacity(): number {
    return _.get(this.json, "Raw.Entity.Components.SCItemThrusterParams.thrustCapacity", 0);
  }

  get fuelBurnRate(): number {
    // Rate is per 10KN, so divide by 1e4 to get the rate per newton
    return _.get(this.json, "Raw.Entity.Components.SCItemThrusterParams.fuelBurnRatePer10KNewton", 0) / 1e4;
  }

  get fuelPushRate(): number {
    return _.get(this.json, "Raw.Entity.Components.SCItemFuelIntakeParams.fuelPushRate", 0);
  }

  get maxThrustFuelBurnRate(): number {
    return this.thrustCapacity * this.fuelBurnRate;
  }

  get quantumDrive(): any {
    return _.get(this.json, "Raw.Entity.Components.SCItemQuantumDriveParams");
  }

  get quantumFuelRequirement(): number {
    // Believe this value is in fuel / Mm, so divide by 1000 to turn it into fuel / m
    return _.get(this.json, "Raw.Entity.Components.SCItemQuantumDriveParams.quantumFuelRequirement", 0) / 1e6;
  }

  get jumpRange(): number {
    return _.get(this.json, "Raw.Entity.Components.SCItemQuantumDriveParams.jumpRange", 0);
  }

  get driveSpeed(): number {
    return _.get(this.json, "Raw.Entity.Components.SCItemQuantumDriveParams.params.driveSpeed", 0);
  }

  get efficiency(): number {
    return 1 / this.quantumFuelRequirement;
  }

  get secondsToArcCorp(): number {
    let speed = this.driveSpeed;
    return distanceBetweenPOandArcCorp / speed;
  }

  get fuelToArcCorp(): number {
    return distanceBetweenPOandArcCorp * this.quantumFuelRequirement;
  }

  get health(): number {
    return _.get(this.json, "Raw.Entity.Components.SHealthComponentParams.Health", 0);
  }

  get degregation(): any {
    return _.get(this.json, "Raw.Entity.Components.SDegradationParams");
  }

  get maxLifetime(): number {
    return _.get(this.json, "Raw.Entity.Components.SDegradationParams.MaxLifetimeHours", 0);
  }

  get damageResistances(): any {
    return _.get(this.json, "Raw.Entity.Components.SDegradationParams.DamageResistances");
  }

  get heatConnection(): any {
    return _.get(this.json, "Raw.Entity.Components.EntityComponentHeatConnection");
  }

  get armor(): any {
    return _.get(this.json, "Raw.Entity.Components.SCItemVehicleArmorParams");
  }

  get cooler(): any {
    return _.get(this.json, "Raw.Entity.Components.SCItemCoolerParams");
  }

  get ammoContainer(): any {
    return _.get(this.json, "Raw.Entity.Components.SAmmoContainerComponentParams");
  }

  get distortion(): any {
    return _.get(this.json, "Raw.Entity.Components.SDistortionParams");
  }

  get emp(): any {
    return _.get(this.json, "Raw.Entity.Components.SCItemEMPParams");
  }

  get missile(): any {
    return _.get(this.json, "Raw.Entity.Components.SCItemMissileParams");
  }

  get miningLaser(): any {
    return _.get(this.json, "Raw.Entity.Components.SEntityComponentMiningLaserParams");
  }

  get weapon(): any {
    return _.get(this.json, "Raw.Entity.Components.SCItemWeaponComponentParams");
  }

  get weaponFireAction(): FireAction {
    return this.getFiringAction(this.json.Raw.Entity.Components.SCItemWeaponComponentParams.fireActions);
  }

  get weaponDamagePerShot(): DamageInfo {
    let ammo = this.ammoDamage;
    let action = this.weaponFireAction;
    let pellets = action.launchParams?.SProjectileLauncher?.pelletCount || 1;

    return {
      DamagePhysical: ammo.DamagePhysical * pellets,
      DamageEnergy: ammo.DamageEnergy * pellets,
      DamageDistortion: ammo.DamageDistortion * pellets,
      DamageThermal: ammo.DamageThermal * pellets,
      DamageBiochemical: ammo.DamageBiochemical * pellets,
      DamageStun: ammo.DamageStun * pellets
    };
  }

  get weaponDetonationDamagePerShot(): DamageInfo {
    let ammo = this.ammoDetonationDamage;
    let action = this.weaponFireAction;
    let pellets = action.launchParams?.SProjectileLauncher?.pelletCount || 1;

    return {
      DamagePhysical: ammo.DamagePhysical * pellets,
      DamageEnergy: ammo.DamageEnergy * pellets,
      DamageDistortion: ammo.DamageDistortion * pellets,
      DamageThermal: ammo.DamageThermal * pellets,
      DamageBiochemical: ammo.DamageBiochemical * pellets,
      DamageStun: ammo.DamageStun * pellets
    };
  }

  get ammo(): AmmoIndexEntry {
    return this.json.ammo as AmmoIndexEntry;
  }

  get ammoDamage(): DamageInfo {
    let ammo = this.ammo;
    return {
      DamagePhysical: ammo.damage.physical,
      DamageEnergy: ammo.damage.energy,
      DamageDistortion: ammo.damage.distortion,
      DamageThermal: ammo.damage.thermal,
      DamageBiochemical: ammo.damage.biochemical,
      DamageStun: ammo.damage.stun
    };
  }

  get ammoDetonationDamage(): DamageInfo {
    let ammo = this.ammo;

    if (!ammo.detonates || !ammo.detonationDamage) return {
      DamagePhysical: 0,
      DamageEnergy: 0,
      DamageDistortion: 0,
      DamageThermal: 0,
      DamageBiochemical: 0,
      DamageStun: 0
    };

    return {
      DamagePhysical: ammo.detonationDamage.physical,
      DamageEnergy: ammo.detonationDamage.energy,
      DamageDistortion: ammo.detonationDamage.distortion,
      DamageThermal: ammo.detonationDamage.thermal,
      DamageBiochemical: ammo.detonationDamage.biochemical,
      DamageStun: ammo.detonationDamage.stun
    };

  }

  get weaponDamagePerSecond(): DamageInfo {
    let perShot = this.weaponDamagePerShot;
    let perShotDetonation = this.weaponDetonationDamagePerShot;
    let action = this.weaponFireAction;

    return {
      DamagePhysical: (perShot.DamagePhysical + perShotDetonation.DamagePhysical) * action.fireRate / 60,
      DamageEnergy: (perShot.DamageEnergy + perShotDetonation.DamageEnergy) * action.fireRate / 60,
      DamageDistortion: (perShot.DamageDistortion + perShotDetonation.DamageDistortion) * action.fireRate / 60,
      DamageThermal: (perShot.DamageThermal + perShotDetonation.DamageThermal) * action.fireRate / 60,
      DamageBiochemical: (perShot.DamageBiochemical + perShotDetonation.DamageBiochemical) * action.fireRate / 60,
      DamageStun: (perShot.DamageStun + perShotDetonation.DamageStun) * action.fireRate / 60
    };
  }

  get isLogoutPoint(): boolean {
    let interactions: any[] = _.get(this.json, "Raw.Entity.Components.SEntityInteractableParams.Interactable.SharedInteractions", []);
    return !!_.find(interactions, x => x.Name.startsWith("SaveGameLogOut"));
  }

  get itemPorts(): IItemPort[] {
    if (this._itemPorts === undefined) {
      let all: IItemPort[] = [];

      let ports: any[] = _.get(this.json, "Raw.Entity.Components.SItemPortContainerComponentParams", []);
      let itemPortPorts: IItemPort[] = _.map(ports, x => new SCItemItemPort(x, this));
      all = all.concat(itemPortPorts);

      this._itemPorts = all;
    }
    return this._itemPorts || [];
  }

  findItemPorts(predicate?: (itemPort: IItemPort) => boolean): IItemPort[] {
    let found: IItemPort[] = [];
    this.itemPorts.forEach(ip => found = found.concat(ip.findItemPorts(predicate)));
    return found;
  }

  // This (wrongly?) assumes that all firing actions in a firing sequence will be the same
  private getFiringAction(fireActionContainer: any): FireAction {
    if (fireActionContainer.SWeaponActionFireSingleParams) return fireActionContainer.SWeaponActionFireSingleParams as FireAction;
    if (fireActionContainer.SWeaponActionFireRapidParams) return fireActionContainer.SWeaponActionFireRapidParams as FireAction;
    if (fireActionContainer.SWeaponActionSequenceParams) return this.getFiringAction(fireActionContainer.SWeaponActionSequenceParams.sequenceEntries[0].weaponAction);
    if (fireActionContainer.SWeaponActionFireChargedParams) return this.getFiringAction(fireActionContainer.SWeaponActionFireChargedParams.weaponAction);
    return { fireRate: 0, heatPerShot: 0, launchParams: { SProjectileLauncher: { ammoCost: 0, pelletCount: 0 } } };
  }

  get Raw(): any {
    return this.json.Raw;
  }
}

export type FireAction = {
  fireRate: number;
  heatPerShot: number;
  launchParams: {
    SProjectileLauncher: {
      ammoCost: number,
      pelletCount: number
    }
  }
}

export type DamageInfo = {
  DamagePhysical: number;
  DamageEnergy: number;
  DamageDistortion: number;
  DamageThermal: number;
  DamageBiochemical: number;
  DamageStun: number;
}

export type AmmoIndexEntry = {
  range: number,
  speed: number,
  detonates: boolean,
  damage: Damage,
  detonationDamage: Damage
}

export type Damage = {
  physical: number,
  energy: number,
  distortion: number,
  thermal: number,
  biochemical: number,
  stun: number
}
