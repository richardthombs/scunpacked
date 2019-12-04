import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ShipListComponent } from "./shiplist/shiplist.component";
import { ShipComponent } from './ship/ship.component';

const routes: Routes = [
  { path: "", component: ShipListComponent },
  { path: "ships/:name", component: ShipComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
