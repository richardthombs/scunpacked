import { Component } from '@angular/core';

import { ItemBaseComponent } from '../ItemBaseComponent';

@Component({
  selector: 'app-shield',
  templateUrl: './shield.component.html',
  styleUrls: ['./shield.component.scss']
})
export class ShieldComponent extends ItemBaseComponent {

  constructor() { super(); }

}
