import { NgModule, Injectable } from '@angular/core';
import { Routes, RouterModule, Resolve } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from 'src/environments/environment';

import { LocalisationService } from './localisation.service';

import { ShiplistPage } from "./shiplist-page/shiplist-page.component";
import { ShipPage } from './ship-page/ship-page.component';
import { CompareShipsPage } from './compare-ships-page/compare-ships-page.component';
import { ItemlistPage } from './itemlist-page/itemlist-page.component';
import { ItemPage } from './item-page/item-page.component';
import { CompareItemsPage } from './compare-items-page/compare-items-page.component';
import { HomePage } from './home-page/home-page.component';
import { ShoplistPage } from './shoplist-page/shoplist-page.component';

@Injectable()
export class LabelsResolver implements Resolve<any> {

  constructor(private $http: HttpClient) { }

  resolve(): Observable<any> | Promise<any> | any {
    return this.$http.get<{ [id: string]: string }>(`${environment.api}/labels.json`).toPromise().then(r => LocalisationService.SetLabels(r));
  }
}

const routes: Routes = [
  { path: "", component: HomePage },
  { path: "ships", component: ShiplistPage, resolve: { labels: LabelsResolver } },
  { path: "ships/compare", component: CompareShipsPage, resolve: { labels: LabelsResolver } },
  { path: "ships/:name", component: ShipPage, resolve: { labels: LabelsResolver } },
  { path: "items", component: ItemlistPage, resolve: { labels: LabelsResolver } },
  { path: "items/compare", component: CompareItemsPage, resolve: { labels: LabelsResolver } },
  { path: "items/:name", component: ItemPage, resolve: { labels: LabelsResolver } },
  { path: "shops", component: ShoplistPage, resolve: { labels: LabelsResolver } }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [LabelsResolver]
})
export class AppRoutingModule { }
