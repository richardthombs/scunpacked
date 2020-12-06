import { Component, OnInit, Query } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, combineLatest } from 'rxjs';
import * as _ from "lodash";

import { ShipIndexEntry } from '../ShipIndexEntry';
import { map } from 'rxjs/operators';

export type doubleGroupedList<T> = {
  [id: string]: {
    [id: string]: T[]
  }
}

@Component({
  selector: 'app-shiplist',
  templateUrl: './shiplist-page.component.html'
})
export class ShiplistPage implements OnInit {

  private subs: Subscription[] = [];

  byRoles: doubleGroupedList<ShipIndexEntry> = {};

  selectedRole: any;

  constructor(private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {

    let data$ = this.route.data.pipe(map(data => {

      console.log(data);

      // Group by role and sub-role, ships will appear in multiple groupings
      this.byRoles = {};
      data.ships.forEach((s: ShipIndexEntry) => {
        s.roles.forEach(r => {
          if (!this.byRoles[r.role]) this.byRoles[r.role] = {};
          if (!this.byRoles[r.role][r.subRole]) this.byRoles[r.role][r.subRole] = [];
          this.byRoles[r.role][r.subRole].push(s);
        });
      });

      return data;
    }));

    combineLatest(data$, this.route.queryParamMap).subscribe(([data, queryParamMap]) => {
      console.log(data, queryParamMap, this.byRoles);

      let role = queryParamMap.get("role");
      if (role) this.selectRole({ key: role, value: this.byRoles[role] });
      else this.router.navigateByUrl("/ships?role=By manufacturer");
    });
  }

  selectRole(role: any): void {
    console.log(role, role.key);
    this.selectedRole = role;
  }

  shipsInRole(role: { [id: string]: ShipIndexEntry[] }): string {
    let ships: string[] = [];
    _.map(role, subRole => ships = ships.concat(_.map(subRole, s => s.ClassName.toLowerCase())));
    return ships.join(",");
  }

  shipsInSubRole(ships: ShipIndexEntry[]): string {
    return _.map(ships, s => s.ClassName.toLowerCase()).join(",");
  }

}
