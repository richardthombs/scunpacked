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
import { FpsItemsResolver } from './FpsItemsResolver';
import { CommoditiesComponent } from './commodities/commodities.component';
import { FpsWeaponsPageComponent } from './fps-weapons-page/fps-weapons-page.component';

const routes: Routes = [
  //{ path: "", component: HomePage, data: { title: "Star Citizen Unpacked" } },
  { path: "", redirectTo: "/ships", pathMatch: "full" },
  { path: "ships", component: ShiplistPage, resolve: { ships: ShipsResolver }, data: { title: "Ships" } },
  { path: "ships/compare", component: CompareShipsPage },
  { path: "ships/:name", component: ShipPage },
  { path: "items", component: ItemlistPage, resolve: { items: ItemsResolver }, data: { title: "Ship equipment" } },
  { path: "items/compare", component: CompareItemsPage },
  { path: "items/:name", component: ItemPage },
  { path: "shops", component: ShoplistPage, data: { title: "Shops and prices" } },
  { path: "commodities", component: CommoditiesComponent, data: { title: "Commodity prices" } },
  { path: "fps-weapons", component: FpsWeaponsPageComponent, resolve: { items: FpsItemsResolver }, data: { title: "FPS gear" } },
  { path: "fps-weapons/compare", component: CompareItemsPage },
  { path: "fps-weapons/:name", component: ItemPage }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule],
  providers: [
    ShipsResolver,
    ItemsResolver,
    FpsItemsResolver
  ]
})
export class AppRoutingModule { }
