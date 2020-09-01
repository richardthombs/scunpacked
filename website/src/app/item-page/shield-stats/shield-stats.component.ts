import { Component, OnInit, Input } from '@angular/core';
import { SCItem } from 'src/app/SCItem';


@Component({
  selector: 'app-shield-stats',
  templateUrl: './shield-stats.component.html',
  styles: [
    ":host { display: block; }"
  ]
})
export class ShieldStatsComponent implements OnInit {

  @Input()
  item!: SCItem;

  constructor() { }

  ngOnInit(): void {
  }

}
