import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { ShiplistPage } from './shiplist-page/shiplist-page.component';
import { ShipPage } from './ship-page/ship-page.component';
import { CompareShipsPage } from './compare-ships-page/compare-ships-page.component';
import { ItemlistPage } from './itemlist-page/itemlist-page.component';
import { ItemPage } from './item-page/item-page.component';
import { CompareItemsPage } from './compare-items-page/compare-items-page.component';
import { HomePage } from './home-page/home-page.component';
import { ShoplistPage } from './shoplist-page/shoplist-page.component';

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
import { NavbarComponent } from './navbar/navbar.component';
import { LocalisationService, LabelsApi } from "./Localisation";
import { StatComponent } from './stat/stat.component';
import { RangeStatComponent } from './rangestat/rangestat.component';
import { CommoditiesComponent } from './commodities/commodities.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { PowerUsageComponent } from './item-page/power-usage/power-usage.component';
import { PowerEmissionsComponent } from './item-page/power-emissions/power-emissions.component';
import { HeatEmissionsComponent } from './item-page/heat-emissions/heat-emissions.component';
import { DurabilityComponent } from './item-page/durability/durability.component';
import { ShieldStatsComponent } from './item-page/shield-stats/shield-stats.component';
import { QuantumDriveComponent } from './item-page/quantum-drive/quantum-drive.component';
import { FpsWeaponsPageComponent } from './fps-weapons-page/fps-weapons-page.component';

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
    HydrogenfuelintakeComponent,
    HomePage,
    ShoplistPage,
    NavbarComponent,
    StatComponent,
    RangeStatComponent,
    CommoditiesComponent,
    PowerUsageComponent,
    PowerEmissionsComponent,
    HeatEmissionsComponent,
    DurabilityComponent,
    ShieldStatsComponent,
    QuantumDriveComponent,
    FpsWeaponsPageComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production })
  ],
  providers: [
    LabelsApi,
    {
      provide: APP_INITIALIZER,
      deps: [LabelsApi],
      useFactory: (labelsApi: LabelsApi) => () => labelsApi.get().then(x => LocalisationService.SetLabels(x)),
      multi: true

    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
