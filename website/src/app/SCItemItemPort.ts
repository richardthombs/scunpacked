import * as _ from 'lodash';

import { SCItem } from './SCItem';
import { IItemPort } from './IItemPort';

export class SCItemItemPort implements IItemPort {

  item: SCItem | undefined;
  itemClass: string | undefined;

  constructor(private json: any, public parentItem?: SCItem) {
  }

  get name(): string {
    return _.get(this.json, "Name", "");
  }

  get minSize(): number {
    return _.get(this.json, "MinSize", 0);
  }

  get maxSize(): number {
    return _.get(this.json, "MaxSize", 0);
  }

  get flags(): string[] {
    return [];
  }

  get types(): string[] {
    let types: string[] = [];
    let superTypes: any[] = _.get(this.json, "Types", []);
    superTypes.forEach(superType => {
      let subTypes: any[] = _.get(superType, "SubTypes", []);
      if (subTypes.length == 0)
        types.push(superType.Type);
      else
        subTypes.forEach(subType => types.push(`${superType.Type}.${subType.value}`));
    });
    return types;
  }

  get displayName(): string {
    let name = _.get(this.json, "DisplayName");
    if (!name) return "";

    if (name.includes(" ")) return name; // Another way to get around ports with already-localised names
    return "itemPort_" + name;
  }

  findItemPorts(predicate?: ((itemPort: IItemPort) => boolean) | undefined): IItemPort[] {
    var found: IItemPort[] = [];
    if (!predicate || predicate(this)) found.push(this);
    if (this.item) found = found.concat(this.item.findItemPorts(predicate));
    return found;
  }

  isEditable(): boolean {
    return !(this.flags.includes("uneditable") || this.flags.includes("$uneditable"));
  }

  isGunHardpoint(): boolean {
    return this.types.includes("WeaponGun.Gun");
  }

  isMissileHardpoint(): boolean {
    return this.types.includes("MissileLauncher.MissileRack");
  }

  isWeaponHardpoint(): boolean {
    return this.isGunHardpoint() || this.isMissileHardpoint();
  }

  isRemoteControlled(): boolean {
    if (this.isWeaponAttachment()) return false;

    // Remote turrets seem to have controllableTags and no UsableDef
    let controllableTags = _.get(this.json, "ControllerDef.controllableTags");
    let usableDef = _.get(this.json, "ControllerDef.UsableDef");
    let remote = controllableTags && !usableDef;
    if (remote) return true;

    if (this.parentItem && this.parentItem.parentPort) return this.parentItem.parentPort.isRemoteControlled();
    return false;
  }

  isManned(): boolean {
    if (this.isWeaponAttachment()) return false;

    // Manned turrets seem to have controllableTags and a UsableDef
    let controllableTags = _.get(this.json, "ControllerDef.controllableTags");
    let usableDef = _.get(this.json, "ControllerDef.UsableDef");

    let manned = controllableTags && usableDef;
    if (manned) return true;

    if (this.parentItem && this.parentItem.parentPort) return this.parentItem.parentPort.isManned();
    return false;
  }

  isPilotControlled(): boolean {
    return !this.isWeaponAttachment() && !this.isManned() && !this.isRemoteControlled();
  }

  isWeaponAttachment(): boolean {
    if (_.find(this.types, x => x.startsWith("WeaponAttachment."))) return true;
    if (this.name == "magazine_attach") return true; // Fix for bad itemport flags
    return false;
  }

  isGimballed(): boolean {
    if (!this.parentItem) return false;

    let fullType = `${this.parentItem.type}.${this.parentItem.subType}`;

    if (fullType == "Turret.NoseMounted") return true;
    if (fullType == "Turret.BallTurret") return true;
    if (fullType == "Turret.GunTurret") return true;
    if (fullType == "Turret.CanardTurret") return true;

    return false;
  }
}
