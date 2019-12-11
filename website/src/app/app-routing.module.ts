import { NgModule, Injectable } from '@angular/core';
import { Routes, RouterModule, Resolve } from '@angular/router';
import { HttpClient } from '@angular/common/http';

import { ShipListComponent } from "./shiplist/shiplist.component";
import { ShipComponent } from './ship/ship.component';
import { LocalisePipe } from './localise.pipe';

import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable()
export class LabelsResolver implements Resolve<any> {

  constructor(private $http: HttpClient) { }

  resolve(): Observable<any> | Promise<any> | any {
    return this.$http.get(`${environment.api}/labels.json`).toPromise().then(r => LocalisePipe.SetLabels(r));
  }
}

const routes: Routes = [
  { path: "", component: ShipListComponent },
  { path: "ships/:name", component: ShipComponent, resolve: { labels: LabelsResolver } }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [LabelsResolver]
})
export class AppRoutingModule { }
