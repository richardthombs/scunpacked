import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from "../../environments/environment";
import * as _ from 'lodash';

@Component({
  selector: 'app-shoplist-page',
  templateUrl: './shoplist-page.component.html',
  styleUrls: ['./shoplist-page.component.scss']
})
export class ShoplistPage implements OnInit {

  constructor(private $http: HttpClient) { }

  shops: any[] = [];
  items: any;

  ngOnInit() {
    this.$http.get<any>(`${environment.api}/shops.json`).subscribe(r => {
      console.log(r);
      r.forEach((s: any) => {
        s.inventory = _.sortBy(s.inventory, ["displayName", "name", "type"]);
        s.collapsed = true;
      });

      this.shops = r;

      let x = _.flatMap(r, shop => _.flatMap(shop.inventory, item => { return { item: item, shop: _.omit(shop, "inventory") } }));
      console.log(x);
      let g = _.groupBy(x, "item.displayName");
      console.log(g);
      this.items = g;
    });
  }

  toggleCollapse(shop: any) {
    shop.collapsed = !shop.collapsed;
  }
}
