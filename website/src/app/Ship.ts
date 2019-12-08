import * as _ from 'lodash';

import { ItemPort } from "./ItemPort";

export class Ship {
  Loadout: ItemPort[] = [];
  Raw: any;

  constructor() { }

  get scu(): number {
    return _.reduce(this.Loadout, (total, ip) => total += ip.loadedItem?.scu || 0, 0)
  }
}
