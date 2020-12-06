import * as _ from 'lodash';

import { StandardisedItem } from './SCItem';

const distanceBetweenPOandArcCorp = 41927351070;

export interface Ship {
  ClassName: string;
  Name: string;
  Description: string;
  Career: string;
  Role: string;
  Size: number;
  Cargo: number;
  Crew: number;
  WeaponCrew: number;
  OperationsCrew: number;
  Mass: number;

  DamageBeforeDestruction: { [part: string]: number };

  QuantumTravel: {
    FuelCapacity: number,
    Range: number,
    Speed: number,
    SpoolTime: number,
    PortOlisarToArcCorpTime: number,
    PortOlisarToArcCorpFuel: number,
    PortOlisarToArcCorpAndBack: number
  };

  Propulsion: {
    FuelCapacity: number;
    FuelIntakeRate: number;
    FuelUsage: {
      Main: number;
      Retro: number;
      Vtol: number;
      Maneuvering: number;
    },
    ManeuveringTimeTillEmpty: number;
  };

  FlightCharacteristics: {
    ScmSpeed: number;
    MaxSpeed: number;
    AccelerationG: {
      Main: number,
      Retro: number,
      Vtol: number,
      Maneuvering: number
    },
    ZeroToMax: number,
    MaxToZero: number,
    ZeroToScm: number,
    ScmToZero: number
  }
}

export interface StandardisedPortSummary {

  PilotHardpoints: StandardisedItemPort[];
  MannedTurrets: StandardisedItemPort[];
  RemoteTurrets: StandardisedItemPort[];
  MiningTurrets: StandardisedItemPort[];
  UtilityTurrets: StandardisedItemPort[];
  MiningHardpoints: StandardisedItemPort[];
  UtilityHardpoints: StandardisedItemPort[];
  MissileRacks: StandardisedItemPort[];
  Countermeasures: StandardisedItemPort[];
  Shields: StandardisedItemPort[];
  PowerPlants: StandardisedItemPort[];
  Coolers: StandardisedItemPort[];
  QuantumDrives: StandardisedItemPort[];
  QuantumFuelTanks: StandardisedItemPort[];
  MainThrusters: StandardisedItemPort[];
  RetroThrusters: StandardisedItemPort[];
  VtolThrusters: StandardisedItemPort[];
  ManeuveringThrusters: StandardisedItemPort[];
  HydrogenFuelTanks: StandardisedItemPort[];
  HydogenFuelIntakes: StandardisedItemPort[];
  InterdictionHardpoints: StandardisedItemPort[];
  CargoGrids: StandardisedItemPort[];
}

export interface StandardisedItemPort {
  PortName: string;
  Size: number;
  Category: string;
  Flags: string[];
  Types: string[];
  Loadout: string;
  InstalledItem: StandardisedItem;
}
