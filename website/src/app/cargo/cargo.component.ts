import { Component } from '@angular/core';

import { ItemBaseComponent } from '../ItemBaseComponent';

@Component({
  selector: 'app-cargo',
  templateUrl: './cargo.component.html',
  styleUrls: ['./cargo.component.scss']
})
export class CargoComponent extends ItemBaseComponent {

  constructor() { super(); }

}
