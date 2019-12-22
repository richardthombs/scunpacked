import { Component, OnInit, Query } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import * as _ from "lodash";

import { environment } from "../../environments/environment";
import { LocalisationService } from '../localisation.service';

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

  byRoles: doubleGroupedList<ShipIndexEntry> = {};
  specials: doubleGroupedList<ShipIndexEntry> = {};
  bySize: doubleGroupedList<ShipIndexEntry> = {};

  selectedRole: any;

  constructor(private $http: HttpClient, private localisationSvc: LocalisationService) { }

  ngOnInit() {
    this.$http.get<ShipIndexEntry[]>(`${environment.api}/ships.json`).subscribe(r => {

      // Fix ships without names
      r.forEach(s => s.name = (s.name == "@LOC_PLACEHOLDER" || s.name == "@LOC_UNINITIALIZED") ? s.className : s.name)

      // Figure out our own roles and sub-roles rather than using CIG's career/roles
      r.forEach(s => {
        s.roles = _.flatMap(this.localisationSvc.getText(s.role).split(" / "), cigRole => {
          if (!s.dogFightEnabled || s.career == "@LOC_PLACEHOLDER" || s.noParts) return { role: "Under development", subRole: "General" };

          if (s.isGroundVehicle) return { role: "Vehicles", subRole: cigRole };
          if (s.isGravlevVehicle) return { role: "Gravlev", subRole: cigRole };

          return this.isRolePrefix(cigRole) ? cigRole.split(" ").filter(rr => !this.isSizePrefix(rr)).map(rr => { return { role: rr, subRole: cigRole }; }) : { role: cigRole, subRole: "General" }
        });

        if (s.isSpaceship) s.roles.push({ role: "Ships by size", subRole: `Size ${s.size || 0}` })
        if (s.isGroundVehicle) s.roles.push({ role: "Vehicles by size", subRole: `Size ${s.size || 0}` })
        if (s.isGravlevVehicle) s.roles.push({ role: "Gravlev by size", subRole: `Size ${s.size || 0}` })
      });

      // Group by role and sub-role, ships will appear in multiple groupings
      r.forEach(s => {
        s.roles.forEach(r => {
          if (!this.byRoles[r.role]) this.byRoles[r.role] = {};
          if (!this.byRoles[r.role][r.subRole]) this.byRoles[r.role][r.subRole] = [];
          this.byRoles[r.role][r.subRole].push(s);
        });
      });

      // Move special groupings
      ["Vehicles", "Gravlev", "Under development"].forEach(
        grouping => {
          this.specials[grouping] = this.byRoles[grouping];
          //delete this.byRoles[grouping];
        }
      );

      // Move into by size groupings
      ["Ships by size", "Vehicles by size", "Gravlev by size"].forEach(
        grouping => {
          this.bySize[grouping] = this.byRoles[grouping];
          //delete this.byRoles[grouping];
        }
      );

      var first = _.sortBy(Object.keys(this.byRoles))[0];
      this.selectRole({ key: first, value: this.byRoles[first] });

    });
  }

  selectRole(role: any): void {
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

  private isRolePrefix(role: string): boolean {
    if (role.startsWith("Light")) return true;
    if (role.startsWith("Medium")) return true;
    if (role.startsWith("Heavy")) return true;
    if (role.startsWith("Stealth")) return true;
    if (role.startsWith("Snub")) return true;
    return false;
  }

  private isSizePrefix(role: string): boolean {
    if (role.startsWith("Light")) return true;
    if (role.startsWith("Medium")) return true;
    if (role.startsWith("Heavy")) return true;
    if (role.startsWith("Snub")) return true;
    return false;
  }

}

interface ShipIndexEntry {
  jsonFilename: string;
  name: string;
  career: string;
  role: string;
  className: string;
  type: string;
  subType: string;
  dogFightEnabled: boolean;
  size?: number;
  isGroundVehicle: boolean;
  isGravlevVehicle: boolean;
  isSpaceship: boolean;
  noParts: boolean;

  // We add these fields as we parse what we download from the API
  roles: { role: string, subRole: string }[];
}
