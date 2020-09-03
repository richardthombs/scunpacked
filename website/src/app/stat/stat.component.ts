import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { SiPipe } from '../si.pipe';

@Component({
  selector: 'app-stat',
  templateUrl: './stat.component.html',
  styles: []
})
export class StatComponent implements OnInit, OnChanges {

  @Input() title: string = "";
  @Input() value: number | boolean = 0;
  @Input() units: string = "";
  @Input() si: boolean = false;
  @Input() decimals?: number = undefined;
  @Input() bool: boolean = false;

  formattedValue: string = "";
  siPrefix: string = "";


  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(): void {
    if (this.value === null) return;

    if (this.bool) {
      this.formattedValue = this.value ? "Yes" : "No";
    }
    else {

      let numeric = this.value as number;

      if (this.si) {
        let siInfo = SiPipe.siPrefix(numeric);
        this.formattedValue = siInfo.value.toFixed(siInfo.value < 10 ? this.decimals || 1 : 0);
        this.siPrefix = siInfo.prefix;
      }
      else {
        this.formattedValue = numeric.toFixed(numeric < 10 ? this.decimals || 1 : 0);
      }

      if (this.decimals === undefined) {
        this.formattedValue = parseFloat(this.formattedValue).toLocaleString();
      }
    }
  }
}
