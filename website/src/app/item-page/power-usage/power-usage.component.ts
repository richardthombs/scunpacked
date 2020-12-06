import { Component, OnInit, Input } from '@angular/core';
import { SCItem, StandardisedItem } from 'src/app/SCItem';

@Component({
  selector: 'app-power-usage',
  templateUrl: './power-usage.component.html',
  styles: [
    ":host { display: block; }"
  ]
})
export class PowerUsageComponent implements OnInit {

  @Input()
  item!: StandardisedItem;

  constructor() { }

  ngOnInit(): void {
  }

}
