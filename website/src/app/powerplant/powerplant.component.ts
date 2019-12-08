import { Component, OnInit, Input } from '@angular/core';
import { Item } from '../Item';
import { SCItem } from '../SCItem';

@Component({
  selector: 'app-powerplant',
  templateUrl: './powerplant.component.html',
  styleUrls: ['./powerplant.component.scss']
})
export class PowerplantComponent implements OnInit {

  @Input()
  item: Item | undefined;

  scitem: SCItem | undefined;

  constructor() { }

  ngOnInit() {
    if (this.item) this.scitem = new SCItem(this.item);
  }

}
