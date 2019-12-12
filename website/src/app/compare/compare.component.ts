import { Component, OnInit } from '@angular/core';

import { ShipService } from '../ship.service';
import { Ship } from '../Ship';


@Component({
  selector: 'app-compare',
  templateUrl: './compare.component.html',
  styleUrls: ['./compare.component.scss']
})
export class CompareComponent implements OnInit {

  ships: any[] | undefined;

  constructor(private shipSvc: ShipService) { }

  ngOnInit() {
    let shipClasses = ["misc_freelancer", "misc_freelancer_max", "drak_cutlass_black", "drak_caterpillar"];

    let shipPromises: Promise<Ship>[] = [];

    for (let i = 0; i < shipClasses.length; i++) {
      shipPromises[i] = this.shipSvc.load(shipClasses[i]);
    }

    Promise.all(shipPromises).then((ships: Ship[]) => {
      this.ships = ships;
    });
  }

}
