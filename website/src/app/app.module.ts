import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ShipListComponent } from './shiplist/shiplist.component';
import { ShipComponent } from './ship/ship.component';
import { ItemComponent } from './item/item.component';
import { NoZeroPipe } from './no-zero.pipe';

@NgModule({
  declarations: [
    AppComponent,
    ShipListComponent,
    ShipComponent,
    ItemComponent,
    NoZeroPipe
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
