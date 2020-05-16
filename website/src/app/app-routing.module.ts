import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ShiplistPage } from "./shiplist-page/shiplist-page.component";
import { ShipPage } from './ship-page/ship-page.component';
import { CompareShipsPage } from './compare-ships-page/compare-ships-page.component';
import { ItemlistPage } from './itemlist-page/itemlist-page.component';
import { ItemPage } from './item-page/item-page.component';
import { CompareItemsPage } from './compare-items-page/compare-items-page.component';
import { HomePage } from './home-page/home-page.component';
import { ShoplistPage } from './shoplist-page/shoplist-page.component';
import { ShipsResolver } from "./ShipsResolver";
import { ItemsResolver } from './ItemsResolver';
import { CommoditiesComponent } from './commodities/commodities.component';

const routes: Routes = [
  { path: "", component: HomePage, data: { title: "Star Citizen Unpacked" } },
  { path: "ships", component: ShiplistPage, resolve: { ships: ShipsResolver }, data: { title: "Ship explorer" } },
  { path: "ships/compare", component: CompareShipsPage },
  { path: "ships/:name", component: ShipPage },
  { path: "items", component: ItemlistPage, resolve: { items: ItemsResolver }, data: { title: "Item explorer" } },
  { path: "items/compare", component: CompareItemsPage },
  { path: "items/:name", component: ItemPage },
  { path: "shops", component: ShoplistPage, data: { title: "Shops and prices" } },
  { path: "commodities", component: CommoditiesComponent, data: { title: "Commodity prices" } }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [
    ShipsResolver,
    ItemsResolver
  ]
})
export class AppRoutingModule { }
