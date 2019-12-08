import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ShipListComponent } from './shiplist/shiplist.component';
import { ShipComponent } from './ship/ship.component';
import { ItemPortComponent } from './itemport/itemport.component';
import { NoZeroPipe } from './no-zero.pipe';
import { ItemGroupComponent } from './item-group/item-group.component';
import { LocalisePipe } from './localise.pipe';
import { PowerplantComponent } from './powerplant/powerplant.component';
import { CargoComponent } from './cargo/cargo.component';

@NgModule({
  declarations: [
    AppComponent,
    ShipListComponent,
    ShipComponent,
    ItemPortComponent,
    NoZeroPipe,
    ItemGroupComponent,
    LocalisePipe,
    PowerplantComponent,
    CargoComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
