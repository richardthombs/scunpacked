import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { SiPipe } from '../si.pipe';

@Component({
  selector: 'app-stat',
  templateUrl: './stat.component.html',
  styles: []
})
export class StatComponent implements OnInit, OnChanges {

  @Input() title: string = "";
  @Input() value: number = 0;
  @Input() units: string = "";
  @Input() si: boolean = false;
  @Input() decimals?: number = undefined;

  formattedValue: string = "";
  siPrefix: string = "";


  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(): void {
    if (this.value === null) return;
    if (this.si) {
      let siInfo = SiPipe.siPrefix(this.value);
      this.formattedValue = siInfo.value.toFixed(siInfo.value < 10 ? this.decimals || 1 : 0);
      this.siPrefix = siInfo.prefix;
    }
    else {
      this.formattedValue = this.value.toFixed(this.value < 10 ? this.decimals || 1 : 0);
    }

    if (this.decimals === undefined) {
      this.formattedValue = parseFloat(this.formattedValue).toLocaleString();
    }
  }

}
