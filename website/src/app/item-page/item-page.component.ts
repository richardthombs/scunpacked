import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

import { environment } from 'src/environments/environment';
import { SCItem } from '../SCItem';

@Component({
  selector: 'app-item-page',
  templateUrl: './item-page.component.html',
  styleUrls: ['./item-page.component.scss']
})
export class ItemPage implements OnInit {

  item: SCItem | undefined;

  constructor(private $http: HttpClient, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {

      let itemClass = params.get("name");
      if (!itemClass) return;

      this.$http.get(`${environment.api}/items/${itemClass}.json`).toPromise().then(r => {
        var item = new SCItem(r);
        this.item = item;
        console.log(item);
      });
    });
  }
}
