import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import * as _ from 'lodash';

import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PriceService {

  itemSubject = new BehaviorSubject<{ [id: string]: any }>({});
  item$ = this.itemSubject.asObservable();

  constructor(private $http: HttpClient) {
    this.$http.get<any>(`${environment.api}/shops.json`).subscribe(r => {
      let mapped = _.flatMap(r, shop => _.flatMap(shop.inventory, item => { return { item: item, shop: _.omit(shop, "inventory") } }));
      let grouped = _.groupBy(mapped, "item.name");
      this.itemSubject.next(grouped);
    });
  }
}
