import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Subject, pipe } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter } from 'rxjs/operators';
import * as _ from 'lodash';

import { environment } from "../../environments/environment";

@Component({
  selector: 'app-shoplist-page',
  templateUrl: './shoplist-page.component.html'
})
export class ShoplistPage implements OnInit {

  constructor(private $http: HttpClient) { }

  shops: any[] = [];
  searchText: string = "";
  grouped: any;
  filtered: any;
  filterChanged: Subject<string> = new Subject<string>();

  ngOnInit() {
    this.filterChanged.pipe(
      debounceTime(300),
      filter(x => x.length >= 3),
      distinctUntilChanged()
    ).subscribe(text => {
      this.searchText = text;
      this.doSearch();
    });

    this.$http.get<any>(`${environment.api}/shops.json`).subscribe(r => {
      r.forEach((s: any) => {
        s.inventory = _.sortBy(s.inventory, ["displayName", "name", "type"]);
        s.collapsed = true;
      });

      this.shops = r;

      let items = _.flatMap(r, shop => _.flatMap(shop.inventory, item => { return { item: item, shop: _.omit(shop, "inventory") } }));
      items.forEach(x => {
        x.item.shopPrice = x.item.basePrice + (x.item.basePrice * x.item.basePriceOffsetPercentage / 100);
        x.item.discPrice = x.item.shopPrice - (x.item.shopPrice * x.item.maxDiscountPercentage / 100);
        x.item.premPrice = x.item.shopPrice + (x.item.shopPrice * x.item.maxPremiumPercentage / 100);
      });
      items = _.sortBy(items, ["item.shopBuysThis", "shop.name"]);
      this.grouped = _.groupBy(items, "item.displayName");
      console.log(_.mapValues(this.grouped, g => {
        return {
          consumable: !g[0].item.type,
          lowestPrice: _.minBy(g, (x: any) => x.item.discPrice),
          highestPrice: _.maxBy(g, (x: any) => x.item.premPrice),
          data: g
        };
      }));

      console.log(this.grouped);

      this.doSearch();
    });
  }

  toggleCollapse(shop: any) {
    shop.collapsed = !shop.collapsed;
  }

  doSearch() {
    this.filtered = _.pickBy(this.grouped, (v, k) => k.toLowerCase().includes(this.searchText.toLowerCase()));
  }
}
