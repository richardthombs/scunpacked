import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import * as _ from 'lodash';

import { environment } from 'src/environments/environment';

import { SCItem } from '../SCItem';

@Component({
  selector: 'app-compare-items-page',
  templateUrl: './compare-items-page.component.html',
  styleUrls: ['./compare-items-page.component.scss']
})
export class CompareItemsPage implements OnInit {

  items: SCItem[] = [];

  private currentSortField: string = "name";
  private currentSortDirection: "asc" | "desc" = "asc";

  constructor(private $http: HttpClient, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.queryParamMap.subscribe(params => {

      let itemsParam: string = params.get("items") || "";
      let itemClasses = itemsParam.split(",");

      let itemPromises: Promise<SCItem>[] = [];

      this.items = [];
      for (let i = 0; i < itemClasses.length; i++) {
        itemPromises[i] = this.$http.get<any>(`${environment.api}/items/${itemClasses[i].toLowerCase()}.json`).toPromise().then(i => { this.items.push(new SCItem(i)); this.applySort(); return i; });
      }

      Promise.all(itemPromises).then((items: SCItem[]) => {
        //this.sortBy(this.currentSortField);
      });

    });
  }

  sortBy(field: string, defaultDirection?: "asc" | "desc") {
    if (field == this.currentSortField) this.currentSortDirection = this.currentSortDirection == "asc" ? "desc" : "asc";
    else this.currentSortDirection = defaultDirection || "asc";
    this.currentSortField = field;
    this.applySort();
  }

  applySort() {
    this.items = _.orderBy(this.items, [i => _.get(i, this.currentSortField)], [this.currentSortDirection]);
  }

}
