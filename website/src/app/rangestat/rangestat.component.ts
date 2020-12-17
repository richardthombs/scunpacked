import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { StandardisedShieldAbsorptionRange } from '../SCItem';
import { SiPipe } from '../si.pipe';

@Component({
  selector: 'app-rangestat',
  templateUrl: './rangestat.component.html',
  styles: []
})
export class RangeStatComponent implements OnChanges {

  @Input() title: string = "";
  @Input() value?: { Minimum: number, Maximum: number };
  @Input() units: string = "";
  @Input() decimals?: number = undefined;
  @Input() percent: boolean = false;

  formattedValue: { Minimum: string, Maximum: string } = { Minimum: "", Maximum: "" };

  ngOnChanges(): void {
    if (this.value === null) return;
    if (this.value === undefined) return;

    if (this.percent) {
      this.value.Minimum = this.value.Minimum * 100;
      this.value.Maximum = this.value.Maximum * 100;
      this.units = "%"
    }

    this.formattedValue.Minimum = this.formatValue(this.value.Minimum);
    this.formattedValue.Maximum = this.formatValue(this.value.Maximum);
  }

  formatValue(value: number, decimals?: number): string {
    let formattedValue = value.toFixed(value < 10 ? this.decimals || 1 : 0);

    if (decimals === undefined) {
      formattedValue = parseFloat(formattedValue).toLocaleString();
    }

    return formattedValue;
  }
}
