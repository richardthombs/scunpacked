import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

import { environment } from 'src/environments/environment';
import { SCItem } from '../SCItem';
import { PriceService } from '../price.service';
import * as _ from 'lodash';
import { LocalisationService } from '../Localisation';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-item-page',
  templateUrl: './item-page.component.html',
  styleUrls: ['./item-page.component.scss']
})
export class ItemPage implements OnInit {

  item: SCItem | undefined;
  prices: any[] = [];
  jsonHref: string = "";

  constructor(private $http: HttpClient, private route: ActivatedRoute, private priceSvc: PriceService, private titleSvc: Title, private localisationSvc: LocalisationService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {

      let itemClass = params.get("name") || "";
      if (!itemClass) return;

      this.jsonHref = `${environment.api}/items/${itemClass}.json`;

      this.$http.get(`${environment.api}/items/${itemClass}.json`).toPromise().then(r => {
        var item = new SCItem(r);

        this.titleSvc.setTitle(`${this.localisationSvc.getText(item.name) || item.className}`);

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
