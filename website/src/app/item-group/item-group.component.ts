import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-itemgroup',
  templateUrl: './item-group.component.html',
  styleUrls: ['./item-group.component.scss']
})
export class ItemGroupComponent implements OnInit {

  @Input()
  groupName: string = "";

  @Input()
  groupItems: { [id: string]: any } = {};

  constructor() { }

  ngOnInit() {
  }

  toggleItems(className: string) {
    this.groupItems[className].showItems = !this.groupItems[className].showItems;
  }
}
