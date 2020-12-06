import { Component, OnInit, Input } from '@angular/core';
import { StandardisedItem } from 'src/app/SCItem';

@Component({
  selector: 'app-heat-emissions',
  templateUrl: './heat-emissions.component.html',
  styles: [
    ":host { display: block; }"
  ]
})
export class HeatEmissionsComponent implements OnInit {

  @Input()
  item!: StandardisedItem;

  constructor() { }

  ngOnInit(): void {
  }

}
