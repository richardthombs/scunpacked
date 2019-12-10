import * as _ from 'lodash';

import { SCItem } from './SCItem';

export interface IItemPort {
  name: string;
  displayName: string;
  minSize: number;
  maxSize: number;
  flags: string[];
  types: string[];
  item: SCItem | undefined;

  findItemPorts(predicate?: (itemPort: IItemPort) => boolean): IItemPort[];
}

export class ItemPort implements IItemPort {
  private json: any;
  private _types: string[] | undefined;

  name: string = "";
  item: SCItem | undefined;

  constructor(jsonItemPort: any, name: string) {
    this.json = jsonItemPort;
    this.name = name;
  }

  get displayName(): string {
    return _.get(this.json, "display_name", "");
  }

  get minSize(): number {
    return _.get(this.json, "minsize", 0);
  }

  get maxSize(): number {
    return _.get(this.json, "maxsize", 0);
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
}


export class SCItemItemPort implements IItemPort {

  item: SCItem | undefined;

  constructor(private json: any) {
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
      if (subTypes.length == 0) types.push(superType.Type);
      else subTypes.forEach(subType => types.push(`${superType.Type}.${subType.value}`));
    });

    return types;
  }

  get displayName(): string {
    return _.get(this.json, "Slot", "");
  }

  findItemPorts(predicate?: ((itemPort: IItemPort) => boolean) | undefined): IItemPort[] {
    throw new Error("Method not implemented.");
  }
}
