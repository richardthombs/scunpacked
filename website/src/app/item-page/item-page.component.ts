import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

import { environment } from 'src/environments/environment';
import { SCItem } from '../SCItem';
import { PriceService } from '../price.service';
import * as _ from 'lodash';

@Component({
  selector: 'app-item-page',
  templateUrl: './item-page.component.html',
  styleUrls: ['./item-page.component.scss']
})
export class ItemPage implements OnInit {

  item: SCItem | undefined;
  prices: any[] = [];

  constructor(private $http: HttpClient, private route: ActivatedRoute, private priceSvc: PriceService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {

      let itemClass = params.get("name") || "";
      if (!itemClass) return;

      this.$http.get(`${environment.api}/items/${itemClass}.json`).toPromise().then(r => {
        var item = new SCItem(r);
        this.item = item;
        console.log(item);

        this.priceSvc.item$.subscribe(r => {
          this.prices = r[itemClass];
        });
      });
    });
  }
}
