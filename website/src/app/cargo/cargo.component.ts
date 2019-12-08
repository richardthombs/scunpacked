import { Component, OnInit, Input, OnChanges } from '@angular/core';

import { Item } from '../Item';
import { SCItem } from '../SCItem';

@Component({
  selector: 'app-cargo',
  templateUrl: './cargo.component.html',
  styleUrls: ['./cargo.component.scss']
})
export class CargoComponent implements OnInit {

  @Input()
  item: SCItem | undefined;

  constructor() { }

  ngOnInit() {
  }

}

