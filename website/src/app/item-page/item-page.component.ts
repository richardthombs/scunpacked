import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

import { environment } from 'src/environments/environment';
import { SCItem, StandardisedItem } from '../SCItem';
import { PriceService } from '../price.service';
import * as _ from 'lodash';
import { LocalisationService } from '../Localisation';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-item-page',
  templateUrl: './item-page.component.html'
})
export class ItemPage implements OnInit {

  item: StandardisedItem | undefined;
  prices: any[] = [];
  jsonHref: string = "";

  constructor(private $http: HttpClient, private route: ActivatedRoute, private priceSvc: PriceService, private titleSvc: Title, private localisationSvc: LocalisationService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {

      let itemClass = params.get("name") || "";
      if (!itemClass) return;

      this.jsonHref = `${environment.api}/v2/items/${itemClass}.json`;

      this.$http.get<StandardisedItem>(this.jsonHref).toPromise().then(item => {
        this.titleSvc.setTitle(item.Name || item.ClassName);
        this.item = item;

        console.log("Item", item);

        this.priceSvc.item$.subscribe(r => {
          this.prices = _.orderBy(r[itemClass], p => this.getActualPrice(p));
        });
      });
    });
  }

  getGrade(grade: number) {
    return String.fromCharCode(65 + grade - 1);
  }

  getActualPrice(p: any) {
    return p.item.basePrice * (1 + (p.item.basePriceOffsetPercentage / 100));
  }

}
