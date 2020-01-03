import * as _ from 'lodash';

import { ItemPort } from './ItemPort';
import { IItemPort } from "./IItemPort";

export class Part {

  private json: any;
  private _itemPort: ItemPort | undefined | null;
  private _parts: Part[] | undefined;

  constructor(jsonPart: any) {
    this.json = jsonPart;
  }

  get itemPort(): ItemPort | undefined {
    if (this._itemPort === null) return undefined;
    if (!this._itemPort) {
      if (this.class == "ItemPort") this._itemPort = new ItemPort(this.json.ItemPort, this.name, this);
      else this._itemPort = null;
    }
    return this._itemPort || undefined;
  }

  get name(): string {
    return _.get(this.json, "name", "");
  }

  get class(): string {
    return _.get(this.json, "class", "");
  }

  get damageMax(): number {
    return _.get(this.json, "damageMax", 0);
  }

  get mass(): number {
    return _.get(this.json, "mass", 0);
  }

  get skipPart(): boolean {
    return _.get(this.json, "skipPart", false);
  }

  get parts(): Part[] {
    if (!this._parts) {
      let parts: any[] = _.get(this.json, "Parts", []);
      this._parts = parts.map(x => new Part(x)).filter(x => !x.skipPart);
    }

    return this._parts;
  }

  findParts(predicate?: (p: Part) => boolean): Part[] {
    let found: Part[] = [];

    if (!predicate || predicate(this)) found.push(this);

    this.parts.forEach(p => {
      var s = p.findParts(predicate);
      found = found.concat(s);
    });

    return found;
  }

  findItemPorts(predicate?: (ip: IItemPort) => boolean): IItemPort[] {
    let found: IItemPort[] = [];
    let parts = this.findParts(p => p.class == "ItemPort" && !!p.itemPort);
    let itemPorts = parts.map(p => <IItemPort>p.itemPort);

    itemPorts.forEach(ip => {
      found = found.concat(ip.findItemPorts(predicate));
    });

    return found;
  }
}
