import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import * as _ from 'lodash';

import { environment } from "../../environments/environment";

@Component({
	selector: 'app-commodities',
	templateUrl: './commodities.component.html',
	styles: []
})
export class CommoditiesComponent implements OnInit {

	commodityList: any[] = [];

	constructor(private $http: HttpClient) { }

	ngOnInit(): void {
		this.$http.get<any>(`${environment.api}/shops.json`).subscribe(r => {

			let commodityList = _(r).flatMap(shop => _.flatMap(shop.inventory, item => {
				let i = item;
				i.shopPrice = i.basePrice + (i.basePrice * i.basePriceOffsetPercentage / 100);
				i.discPrice = i.shopPrice - (i.shopPrice * i.maxDiscountPercentage / 100);
				i.premPrice = i.shopPrice + (i.shopPrice * i.maxPremiumPercentage / 100);

				return { item: i, shop: _.omit(shop, "inventory") }
			}))
				.filter((x: any) => x.item.type === undefined)
				.sortBy("item.displayName", "shop.name")
				.value();

			let commodities: { [id: string]: any } = {};
			commodityList.forEach(c => {
				let name = c.item.displayName || c.item.name;
				commodities[name] = commodities[name] || { sells: [], buys: [] };
				if (c.item.shopSellsThis) commodities[name].sells.push(c);
				if (c.item.shopBuysThis) commodities[name].buys.push(c);
			});

			Object.keys(commodities).forEach(x => {
				if (commodities[x].sells.length == 0 || commodities[x].buys.length == 0) delete commodities[x];
			});

			_.map(commodities, c => {
				c.bestSellPrice = _.minBy(c.sells, (x: any) => x.item.discPrice).item.discPrice;
				c.bestBuyPrice = _.maxBy(c.buys, (x: any) => x.item.premPrice).item.premPrice;
				c.profitPerUnit = c.bestBuyPrice - c.bestSellPrice;
			});

			commodityList = _(commodities).map((v, k) => _.merge(v, { name: k }))
				.orderBy(["profitPerUnit"], ["desc"])
				.value();

			console.log(commodityList);

			this.commodityList = commodityList;
		});
	}
}
