import { Component } from '@angular/core';

import { ItemBaseComponent } from '../ItemBaseComponent';

@Component({
  selector: 'app-hydrogenfueltank',
  templateUrl: './hydrogenfueltank.component.html',
  styleUrls: ['./hydrogenfueltank.component.scss']
})
export class HydrogenfueltankComponent extends ItemBaseComponent {

  constructor() { super(); }

}
