import { Input } from '@angular/core';

import { SCItem } from './SCItem';

export class ItemBaseComponent {
  @Input()
  item: SCItem | undefined;
}
