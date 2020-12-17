import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { StandardisedShieldAbsorptionRange } from '../SCItem';
import { SiPipe } from '../si.pipe';

@Component({
  selector: 'app-stat',
  templateUrl: './stat.component.html',
  styles: []
})
export class StatComponent implements OnChanges {

  @Input() title: string = "";
  @Input() value?: number | boolean;
  @Input() units: string = "";
  @Input() si: boolean = false;
  @Input() decimals?: number = undefined;
  @Input() bool: boolean = false;
  @Input() percent: boolean = false;

  formattedValue: string = "";
  siPrefix: string = "";

  ngOnChanges(): void {
    if (this.value === null) return;
    if (this.value === undefined) return;
    if (this.value == Infinity) this.formattedValue = "∞";
    else if (this.bool) this.formattedValue = this.value ? "Yes" : "No";
    else if (this.percent) {
      this.formattedValue = `${this.value as number * 100}`;
      this.units = "%";
    }
    else {
      let numeric = this.value as number;
      if (numeric.toFixed) {
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
      else {
        this.formattedValue = this.value.toString();
      }
    }
  }
}
