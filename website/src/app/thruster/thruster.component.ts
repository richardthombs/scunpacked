import { Component } from '@angular/core';

import { ItemBaseComponent } from '../ItemBaseComponent';

@Component({
  selector: 'app-thruster',
  templateUrl: './thruster.component.html',
  styleUrls: ['./thruster.component.scss']
})
export class ThrusterComponent extends ItemBaseComponent {

  constructor() { super(); }

}
