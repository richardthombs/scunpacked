import { Component, OnInit, Query } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import * as _ from "lodash";

import { environment } from "../../environments/environment";

import { LocalisationService } from '../localisation.service';
import { ShipIndexEntry } from '../ShipIndexEntry';

export type doubleGroupedList<T> = {
  [id: string]: {
    [id: string]: T[]
  }
}

@Component({
  selector: 'app-shiplist',
  templateUrl: './shiplist-page.component.html',
  styleUrls: ['./shiplist-page.component.scss']
})
export class ShiplistPage implements OnInit {

  private subs: Subscription[] = [];

  byRoles: doubleGroupedList<ShipIndexEntry> = {};

  selectedRole: any;

  constructor(private $http: HttpClient, private localisationSvc: LocalisationService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.subs.push(this.route.data.subscribe(data => {

      this.byRoles = {};

      // Group by role and sub-role, ships will appear in multiple groupings
      data.ships.forEach((s: ShipIndexEntry) => {
        s.roles.forEach(r => {
          if (!this.byRoles[r.role]) this.byRoles[r.role] = {};
          if (!this.byRoles[r.role][r.subRole]) this.byRoles[r.role][r.subRole] = [];
          this.byRoles[r.role][r.subRole].push(s);
        });
      });
    }));

    this.subs.push(this.route.queryParamMap.subscribe(r => {
      console.log(r);
      let role = r.get("role");
      if (role) this.selectRole({ key: role, value: this.byRoles[role] });
      else {
        this.router.navigateByUrl("/ships?role=Combat");
      }
    }));
  }

  selectRole(role: any): void {
    console.log(role, role.key);
    this.selectedRole = role;
  }

  shipsInRole(role: { [id: string]: ShipIndexEntry[] }): string {
    let ships: string[] = [];
    _.map(role, subRole => ships = ships.concat(_.map(subRole, s => s.className.toLowerCase())));
    return ships.join(",");
  }

  shipsInSubRole(ships: ShipIndexEntry[]): string {
    return _.map(ships, s => s.className.toLowerCase()).join(",");
  }

}
