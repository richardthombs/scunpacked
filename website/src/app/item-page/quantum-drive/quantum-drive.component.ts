import { Component, OnInit, Input } from '@angular/core';
import { SCItem, StandardisedItem } from 'src/app/SCItem';

@Component({
  selector: 'app-quantum-drive',
  templateUrl: './quantum-drive.component.html',
  styles: [
    `
      :host {
        display: block;
      }
    `
  ]
})
export class QuantumDriveComponent implements OnInit {

  @Input()
  item!: StandardisedItem;

  constructor() { }

  ngOnInit(): void {
  }

}
