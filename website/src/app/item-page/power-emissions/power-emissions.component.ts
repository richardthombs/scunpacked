import { Component, OnInit, Input } from '@angular/core';
import { SCItem } from 'src/app/SCItem';

@Component({
  selector: 'app-power-emissions',
  templateUrl: './power-emissions.component.html',
  styles: [
    ":host { display: block; }"
  ]
})
export class PowerEmissionsComponent implements OnInit {

  @Input()
  item!: SCItem;

  constructor() { }

  ngOnInit(): void {
  }

}
