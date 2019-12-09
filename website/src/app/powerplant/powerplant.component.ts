import { Component } from '@angular/core';

import { ItemBaseComponent } from '../ItemBaseComponent';

@Component({
  selector: 'app-powerplant',
  templateUrl: './powerplant.component.html',
  styleUrls: ['./powerplant.component.scss']
})
export class PowerplantComponent extends ItemBaseComponent {

  constructor() { super(); }

}
