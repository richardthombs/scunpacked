import * as _ from 'lodash';

import { SCItem } from './SCItem';
import { IItemPort } from './IItemPort';

export class SCItemItemPort implements IItemPort {

  item: SCItem | undefined;
  itemClass: string | undefined;

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
      if (subTypes.length == 0)
        types.push(superType.Type);
      else
        subTypes.forEach(subType => types.push(`${superType.Type}.${subType.value}`));
    });
    return types;
  }

  get displayName(): string {
    return _.get(this.json, "Slot", "");
  }

  findItemPorts(predicate?: ((itemPort: IItemPort) => boolean) | undefined): IItemPort[] {
    var found: IItemPort[] = [];
    if (!predicate || predicate(this)) found.push(this);
    if (this.item) found = found.concat(this.item.findItemPorts(predicate));
    return found;
  }
}
