import * as _ from 'lodash';

import { SCItem } from './SCItem';
import { IItemPort } from './IItemPort';
import { Part } from './Part';

export class ItemPort implements IItemPort {
  private _types: string[] | undefined;

  constructor(private json: any, public name: string, public parentPart: Part) {
  }

  item: SCItem | undefined;
  itemClass: string | undefined;

  get displayName(): string {
    let name = _.get(this.json, "display_name") || this.name;
    if (!name) return "";
    if (name.includes(" ")) return name; // Another way to get around ports with already-localised names

    return "itemPort_" + name;
  }

  get minSize(): number {
    return Math.min(_.get(this.json, "minsize", 0), 10); // Idris has size 9999 itemports
  }

  get maxSize(): number {
    return Math.min(_.get(this.json, "maxsize", 0), 10); // Idris has size 9999 itemports
  }

  get flags(): string[] {
    return _.filter(_.get(this.json, "flags", "").split(" "), x => !!x);
  }

  get types(): string[] {
    if (!this._types) {
      let types: string[] = [];
      _.get(this.json, "Types", []).forEach((json: any) => {
        let superType = _.get(json, "type", "");
        if (!superType)
          return;
        let subTypes: string[] = _.filter(_.get(json, "subtypes", "").split(","), x => !!x);
        if (!subTypes.length)
          types.push(superType);
        else
          subTypes.forEach(sub => types.push(`${superType}.${sub}`));
      });
      this._types = types;
    }
    return this._types;
  }

  findItemPorts(predicate?: (itemPort: IItemPort) => boolean): IItemPort[] {
    let found: IItemPort[] = [];

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

  isOtherHardpoint(): boolean {
    return this.types.includes("EMP");
  }

  isWeaponHardpoint(): boolean {
    return this.isGunHardpoint() || this.isMissileHardpoint() || this.isOtherHardpoint();
  }

  isRemoteControlled(): boolean {
    // Remote turrets seem to have controllableTags and no UsableDef
    let controllableTags = _.get(this.json, "ControllerDef.controllableTags");
    let usableDef = _.get(this.json, "ControllerDef.UsableDef");
    return controllableTags && !usableDef;
  }

  isManned(): boolean {
    // Manned turrets seem to have controllableTags and a UsableDef
    let controllableTags = _.get(this.json, "ControllerDef.controllableTags");
    let usableDef = _.get(this.json, "ControllerDef.UsableDef");

    return controllableTags && usableDef;
  }

  isPilotControlled(): boolean {
    return !this.isWeaponAttachment() && !this.isManned() && !this.isRemoteControlled();
  }

  isWeaponAttachment(): boolean {
    return !!_.find(this.types, x => x.startsWith("WeaponAttachment."));
  }

  isGimballed(): boolean {
    return false;
  }

}
