import * as _ from 'lodash';

import { Item } from './Item';

export class SCItem {

  constructor(private item: Item) {
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

  get Raw(): any {
    return this.item.Raw;
  }
}
