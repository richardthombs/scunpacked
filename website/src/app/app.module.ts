import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { ShiplistPage } from './shiplist/shiplist.component';
import { ShipPage } from './ship/ship.component';
import { CompareShipsPage } from './compare-ships-page/compare-ships-page.component';
import { ItemlistPage } from './itemlist/itemlist.component';
import { ItemPage } from './item-page/item-page.component';
import { CompareItemsPage } from './compare-items-page/compare-items-page.component';

import { ItemPortComponent } from './itemport/itemport.component';
import { ItemGroupComponent } from './itemgroup/itemgroup.component';
import { PowerplantComponent } from './powerplant/powerplant.component';
import { CargoComponent } from './cargo/cargo.component';
import { ShieldComponent } from './shield/shield.component';
import { QuantumdriveComponent } from './quantumdrive/quantumdrive.component';
import { QuantumfueltankComponent } from './quantumfueltank/quantumfueltank.component';
import { ThrusterComponent } from './thruster/thruster.component';
import { HydrogenfueltankComponent } from './hydrogenfueltank/hydrogenfueltank.component';
import { HydrogenfuelintakeComponent } from './hydrogenfuelintake/hydrogenfuelintake.component';

import { DistancePipe } from './distance.pipe';
import { SpeedPipe } from './speed.pipe';
import { SiPipe } from './si.pipe';
import { LocalisePipe } from './localise.pipe';
import { NoZeroPipe } from './no-zero.pipe';

@NgModule({
  declarations: [
    AppComponent,
    ShiplistPage,
    ShipPage,
    CompareShipsPage,
    ItemlistPage,
    ItemPage,
    CompareItemsPage,
    ItemPortComponent,
    ItemGroupComponent,
    PowerplantComponent,
    CargoComponent,
    ShieldComponent,
    QuantumdriveComponent,
    NoZeroPipe,
    LocalisePipe,
    DistancePipe,
    SpeedPipe,
    SiPipe,
    QuantumfueltankComponent,
    ThrusterComponent,
    HydrogenfueltankComponent,
    HydrogenfuelintakeComponent
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
