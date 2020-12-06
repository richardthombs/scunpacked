import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-itemport',
  templateUrl: './itemport.component.html',
  encapsulation: ViewEncapsulation.None
})
export class ItemPortComponent implements OnInit {

  @Input()
  itemPort: any;

  constructor() { }

  ngOnInit() {
  }
}
