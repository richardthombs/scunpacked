import { Component, OnInit, Input } from '@angular/core';
import { SCItem } from '../SCItem';

@Component({
  selector: 'app-shield',
  templateUrl: './shield.component.html',
  styleUrls: ['./shield.component.scss']
})
export class ShieldComponent implements OnInit {

  @Input()
  item: SCItem | undefined;

  constructor() { }

  ngOnInit() {
  }

}
