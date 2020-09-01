import { Component, OnInit, Input } from '@angular/core';
import { SCItem } from 'src/app/SCItem';

@Component({
  selector: 'app-power-usage',
  templateUrl: './power-usage.component.html',
  styles: [
    ":host { display: block; }"
  ]
})
export class PowerUsageComponent implements OnInit {

  @Input()
  item!: SCItem;

  constructor() { }

  ngOnInit(): void {
  }

}
