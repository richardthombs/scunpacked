import { Component, OnInit, Input } from '@angular/core';
import { SCItem } from 'src/app/SCItem';

@Component({
  selector: 'app-durability',
  templateUrl: './durability.component.html',
  styles: [
    ":host { display: block; }"
  ]
})
export class DurabilityComponent implements OnInit {

  @Input()
  item!: SCItem;

  constructor() { }

  ngOnInit(): void {
  }

}
