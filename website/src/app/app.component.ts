import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
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
    "https://images.squarespace-cdn.com/content/v1/5da95ebb5c5f411c6a05ad9e/1572216865869-WS24CQ0YA2IPEILWQX4J/ke17ZwdGBToddI8pDm48kPTrHXgsMrSIMwe6YW3w1AZ7gQa3H78H3Y0txjaiv_0fDoOvxcdMmMKkDsyUqMSsMWxHk725yiiHCCLfrh8O1z4YTzHvnKhyp6Da-NYroOW3ZGjoBKy3azqku80C789l0k5fwC0WRNFJBIXiBeNI5fKTrY37saURwPBw8fO2esROAxn-RKSrlQamlL27g22X2A/ScreenShot0027.jpg?format=1500w"
  ]
  background: string = "";

  constructor() { }

  ngOnInit() {
    this.background = `url(${this.backgrounds[Math.floor(Math.random() * this.backgrounds.length)]})`;
  }

}
