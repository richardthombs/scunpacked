import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-itemport',
  templateUrl: './itemport.component.html',
  styleUrls: ['./itemport.component.scss']
})
export class ItemPortComponent implements OnInit {

  @Input()
  itemPort: any;

  constructor() { }

  ngOnInit() {
  }

}


