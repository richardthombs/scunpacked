import { Component, OnInit } from '@angular/core';
import { SwUpdate } from '@angular/service-worker';
import { Router, NavigationEnd, Data, ActivatedRoute } from '@angular/router';
import { filter, map, mergeMap } from 'rxjs/operators';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  private backgrounds: string[] = [
    "https://media.robertsspaceindustries.com/9qk5ii6kxbs5f/channel_item_full.jpg",
    "https://media.robertsspaceindustries.com/hamcauwix52wd/channel_item_full.jpg",
    "https://media.robertsspaceindustries.com/27a45m9q5iinb/channel_item_full.jpg",
    "https://robertsspaceindustries.com/media/4m5eop2jvgs6ir/source/Valk1.png",
    "https://robertsspaceindustries.com/media/xkgpg49opojvor/channel_item_full/Source-6.jpg",
    "https://robertsspaceindustries.com/media/8t2ixvajktxtcr/channel_item_full/DRAK_Kraken_Promo_landed04_Grey_PJ03.jpg",
    "https://robertsspaceindustries.com/media/qtrq7xu80ldnfr/channel_item_full/0051.jpg",
    "https://robertsspaceindustries.com/media/12a5kh5pemls2r/channel_item_full/Pisces_C8_Microtech.jpg",
    "https://robertsspaceindustries.com/media/h0ijcnfkb62x2r/channel_item_full/ISC_THUMBNAIL-Nologo.jpg",
    "https://robertsspaceindustries.com/media/83tmncuyta86qr/channel_item_full/Flight.jpg",
    "https://images.squarespace-cdn.com/content/v1/5da95ebb5c5f411c6a05ad9e/1575195490299-MLBJKVOA4M6V8PY0U1OS/ke17ZwdGBToddI8pDm48kPTrHXgsMrSIMwe6YW3w1AZ7gQa3H78H3Y0txjaiv_0fDoOvxcdMmMKkDsyUqMSsMWxHk725yiiHCCLfrh8O1z4YTzHvnKhyp6Da-NYroOW3ZGjoBKy3azqku80C789l0k5fwC0WRNFJBIXiBeNI5fKTrY37saURwPBw8fO2esROAxn-RKSrlQamlL27g22X2A/ScreenShot0274.jpg?format=1500w",
    "https://images.squarespace-cdn.com/content/v1/5da95ebb5c5f411c6a05ad9e/1572216865869-WS24CQ0YA2IPEILWQX4J/ke17ZwdGBToddI8pDm48kPTrHXgsMrSIMwe6YW3w1AZ7gQa3H78H3Y0txjaiv_0fDoOvxcdMmMKkDsyUqMSsMWxHk725yiiHCCLfrh8O1z4YTzHvnKhyp6Da-NYroOW3ZGjoBKy3azqku80C789l0k5fwC0WRNFJBIXiBeNI5fKTrY37saURwPBw8fO2esROAxn-RKSrlQamlL27g22X2A/ScreenShot0027.jpg?format=1500w",
    "https://robertsspaceindustries.com/media/rtmzn4ziowedbr/source/Star_Citizen-Portrait-42.jpg",
    "https://robertsspaceindustries.com/media/1c4p9hlgaq3cjr/source/CloudImperiumGames_StarCitizen_CalliopeProgress.png",
    "https://robertsspaceindustries.com/media/ajdt01mm8dccdr/source/Squadron-42-Star-Citizen-Screenshot-20200130-03244964.jpg"
  ]
  background: string = "";

  constructor(private swUpdate: SwUpdate, private router: Router, private route: ActivatedRoute, private titleSvc: Title) { }

  ngOnInit() {
    this.background = `url(${this.backgrounds[Math.floor(Math.random() * this.backgrounds.length)]})`;

    this.swUpdate.available.subscribe(event => {
      console.log("SwUpdate.available", event);
      this.swUpdate.activateUpdate().then(() => document.location.reload());
    });
    this.swUpdate.activated.subscribe(x => console.log("SwUpdate.activated", x));

    this.router.events.pipe(
      filter(e => e instanceof NavigationEnd),
      map(() => this.route),
      map(route => {
        while (route.firstChild) route = route.firstChild;
        return route;
      }),
      mergeMap(route => route.data)
    ).subscribe((data: Data) => {
      let title = data.title;
      if (title) this.titleSvc.setTitle(title);
    });
  }
}
