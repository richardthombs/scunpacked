import { Input, Directive } from '@angular/core';

import { SCItem } from './SCItem';

@Directive()
export class ItemBaseComponent {
  @Input()
  item: SCItem | undefined;
}
