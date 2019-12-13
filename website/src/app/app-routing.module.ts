import { NgModule, Injectable } from '@angular/core';
import { Routes, RouterModule, Resolve } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from 'src/environments/environment';

import { ShipListComponent } from "./shiplist/shiplist.component";
import { ShipComponent } from './ship/ship.component';
import { CompareComponent } from './compare/compare.component';
import { LocalisationService } from './localisation.service';


@Injectable()
export class LabelsResolver implements Resolve<any> {

  constructor(private $http: HttpClient) { }

  resolve(): Observable<any> | Promise<any> | any {
    return this.$http.get<{ [id: string]: string }>(`${environment.api}/labels.json`).toPromise().then(r => LocalisationService.SetLabels(r));
  }
}

const routes: Routes = [
  { path: "", component: ShipListComponent, resolve: { labels: LabelsResolver } },
  { path: "ships/:name", component: ShipComponent, resolve: { labels: LabelsResolver } },
  { path: "compare", component: CompareComponent, resolve: { labels: LabelsResolver } }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [LabelsResolver]
})
export class AppRoutingModule { }
