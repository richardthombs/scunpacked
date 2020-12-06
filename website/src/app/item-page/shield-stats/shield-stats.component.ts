import { Component, OnInit, Input } from '@angular/core';
import { SCItem, StandardisedItem } from 'src/app/SCItem';


@Component({
  selector: 'app-shield-stats',
  templateUrl: './shield-stats.component.html',
  styles: [
    ":host { display: block; }"
  ]
})
export class ShieldStatsComponent implements OnInit {

  @Input()
  item!: StandardisedItem;

  constructor() { }

  ngOnInit(): void {
  }

}
