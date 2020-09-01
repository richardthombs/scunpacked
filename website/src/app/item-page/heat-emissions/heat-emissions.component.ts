import { Component, OnInit, Input } from '@angular/core';
import { SCItem } from 'src/app/SCItem';

@Component({
  selector: 'app-heat-emissions',
  templateUrl: './heat-emissions.component.html',
  styles: [
    ":host { display: block; }"
  ]
})
export class HeatEmissionsComponent implements OnInit {

  @Input()
  item!: SCItem;

  constructor() { }

  ngOnInit(): void {
  }

}
