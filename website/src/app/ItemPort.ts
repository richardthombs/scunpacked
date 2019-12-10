import * as _ from 'lodash';

import { SCItem } from './SCItem';
import { IItemPort } from './IItemPort';

export class ItemPort implements IItemPort {
  private _types: string[] | undefined;

  constructor(private json: any, public name: string) {
  }

  item: SCItem | undefined;
  itemClass: string | undefined;

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
