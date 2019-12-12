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
import { ShieldComponent } from './shield/shield.component';
import { QuantumdriveComponent } from './quantumdrive/quantumdrive.component';
import { DistancePipe } from './distance.pipe';
import { SpeedPipe } from './speed.pipe';
import { SiPipe } from './si.pipe';
import { QuantumfueltankComponent } from './quantumfueltank/quantumfueltank.component';
import { ThrusterComponent } from './thruster/thruster.component';
import { HydrogenfueltankComponent } from './hydrogenfueltank/hydrogenfueltank.component';
import { HydrogenfuelintakeComponent } from './hydrogenfuelintake/hydrogenfuelintake.component';
import { CompareComponent } from './compare/compare.component';

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
    CargoComponent,
    ShieldComponent,
    QuantumdriveComponent,
    DistancePipe,
    SpeedPipe,
    SiPipe,
    QuantumfueltankComponent,
    ThrusterComponent,
    HydrogenfueltankComponent,
    HydrogenfuelintakeComponent,
    CompareComponent
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
