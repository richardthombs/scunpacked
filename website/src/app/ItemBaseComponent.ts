import { Input, Directive } from '@angular/core';

import { StandardisedItem } from './SCItem';

@Directive()
export class ItemBaseComponent {
  @Input()
  item: StandardisedItem | undefined;
}
