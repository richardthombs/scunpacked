import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import * as _ from 'lodash';

import { environment } from 'src/environments/environment';

import { SCItem } from '../SCItem';
import { LocalisationService } from '../Localisation';
import { ComparisonGroup, ComparisonField } from '../Comparisons';

@Component({
  selector: 'app-compare-items-page',
  templateUrl: './compare-items-page.component.html',
  styleUrls: ['./compare-items-page.component.scss']
})
export class CompareItemsPage implements OnInit {

  fields: ComparisonGroup<SCItem>[] = [
    new ComparisonGroup({
      title: "",
      visibleFn: () => true,
      fields: [
        new ComparisonField({ title: "Name", valueFn: i => this.localisationSvc.getText(i.name, i.className), linkFn: i => `/items/${i.className.toLowerCase()}` }),
        new ComparisonField({ title: "Size", valueFn: i => i.size, formatFn: v => `Size ${v}`, compareFn: undefined }),
      ],
    }),
    new ComparisonGroup({
      title: "Quantum speed",
      visibleFn: items => !!_.find(items, i => i.quantumDrive),
      fields: [
        new ComparisonField({ title: "PO to ArcCorp (time)", units: "s", valueFn: i => i.secondsToArcCorp, formatFn: v => typeof v === "number" ? `${Math.floor(v / 60)}m ${Math.round(v % 60)}s` : "?" }),
        new ComparisonField({ title: "Quantum speed", units: "m/s", siPrefix: true, valueFn: i => i.driveSpeed, sortDirection: "desc" }),
        new ComparisonField({ title: "Phase 1 acceleration", units: "m/s", valueFn: i => _.get(i, "quantumDrive.params.stageOneAccelRate"), sortDirection: "desc" }),
        new ComparisonField({ title: "Phase 2 acceleration", units: "m/s", siPrefix: true, decimals: 1, valueFn: i => _.get(i, "quantumDrive.params.stageTwoAccelRate"), sortDirection: "desc" }),
        new ComparisonField({ title: "Spool up time", units: "s", siPrefix: true, decimals: 1, valueFn: i => _.get(i, "quantumDrive.params.spoolUpTime") }),
      ]
    }),
    new ComparisonGroup({
      title: "Quantum efficiency",
      visibleFn: items => !!_.find(items, i => i.quantumDrive),
      fields: [
        new ComparisonField({ title: "PO to ArcCorp (fuel)", units: "l", valueFn: i => i.fuelToArcCorp }),
        new ComparisonField({ title: "Efficiency", units: "m/l", siPrefix: true, valueFn: i => 1 / i.quantumFuelRequirement, sortDirection: "desc" }),
        new ComparisonField({ title: "Quantum fuel requirement", units: "l/Gm", valueFn: i => i.quantumFuelRequirement * 1e9 })
      ]
    }),
    new ComparisonGroup({
      title: "Shield strength",
      visibleFn: items => !!_.find(items, i => i.shieldGenerator),
      fields: [
        new ComparisonField({ title: "HP", units: "HP", valueFn: i => i.shieldGenerator.MaxShieldHealth, sortDirection: "desc" }),
        new ComparisonField({ title: "Regeneration", units: "HP/s", valueFn: i => i.shieldGenerator.MaxShieldRegen, sortDirection: "desc" }),
        new ComparisonField({ title: "Decay ratio", decimals: 2, valueFn: i => i.shieldGenerator.DecayRatio }),
        new ComparisonField({ title: "Downed delay", decimals: 1, valueFn: i => i.shieldGenerator.DownedRegenDelay }),
        new ComparisonField({ title: "Damaged delay", decimals: 1, valueFn: i => i.shieldGenerator.DamagedRegenDelay }),
      ]
    }),
    new ComparisonGroup({
      title: "Shield absorption",
      visibleFn: items => !!_.find(items, i => i.shieldGenerator),
      fields: [
        new ComparisonField({ title: "Physical", decimals: 2, valueFn: i => i.shieldGenerator.ShieldAbsorption[0].Max, sortDirection: "desc" }),
        new ComparisonField({ title: "Energy", decimals: 2, valueFn: i => i.shieldGenerator.ShieldAbsorption[1].Max, sortDirection: "desc" }),
        new ComparisonField({ title: "Distortion", decimals: 2, valueFn: i => i.shieldGenerator.ShieldAbsorption[2].Max, sortDirection: "desc" }),
        new ComparisonField({ title: "Thermal", decimals: 2, valueFn: i => i.shieldGenerator.ShieldAbsorption[3].Max, sortDirection: "desc" }),
        new ComparisonField({ title: "Biochemical", decimals: 2, valueFn: i => i.shieldGenerator.ShieldAbsorption[4].Max, sortDirection: "desc" }),
      ]
    }),
    new ComparisonGroup({
      title: "Armor shielding",
      visibleFn: items => !!_.find(items, i => !!i.armor),
      fields: [
        new ComparisonField({ title: "Infrared multiplier", decimals: 2, valueFn: i => i.armor.signalInfrared }),
        new ComparisonField({ title: "Electromagnetic multiplier", decimals: 2, valueFn: i => i.armor.signalElectromagnetic }),
        new ComparisonField({ title: "Cross section multiplier", decimals: 2, valueFn: i => i.armor.signalCrossSection }),
      ]
    }),
    new ComparisonGroup({
      title: "Armor resistances",
      visibleFn: items => !!_.find(items, i => !!i.armor),
      fields: [
        new ComparisonField({ title: "Physical", decimals: 2, valueFn: i => i.armor.damageMultiplier.DamageInfo.DamagePhysical }),
        new ComparisonField({ title: "Energy", decimals: 2, valueFn: i => i.armor.damageMultiplier.DamageInfo.DamageEnergy }),
        new ComparisonField({ title: "Distortion", decimals: 2, valueFn: i => i.armor.damageMultiplier.DamageInfo.DamageDistortion }),
        new ComparisonField({ title: "Thermal", decimals: 2, valueFn: i => i.armor.damageMultiplier.DamageInfo.DamageThermal }),
        new ComparisonField({ title: "Biochemical", decimals: 2, valueFn: i => i.armor.damageMultiplier.DamageInfo.DamageBiochemical }),
      ]
    }),
    new ComparisonGroup({
      title: "Cooling",
      visibleFn: items => !!_.find(items, i => !!i.cooler),
      fields: [
        new ComparisonField({ title: "Cooling rate", siPrefix: true, units: "W/s", valueFn: i => i.cooler.CoolingRate, sortDirection: "desc" }),
        new ComparisonField({ title: "IR suppression factor", decimals: 2, valueFn: i => i.cooler.SuppressionIRFactor }),
        new ComparisonField({ title: "Heat suppression factor", decimals: 2, valueFn: i => i.cooler.SuppressionHeatFactor }),
      ]
    }),
    new ComparisonGroup({
      title: "EMP",
      visibleFn: items => !!_.find(items, i => !!i.emp),
      fields: [
        new ComparisonField({ title: "Charge time", units: "s", valueFn: i => i.emp.chargeTime }),
        new ComparisonField({ title: "Distortion damage", units: "HP", valueFn: i => i.emp.distortionDamage, sortDirection: "desc" }),
        new ComparisonField({ title: "EMP radius", units: "m", valueFn: i => i.emp.empRadius, sortDirection: "desc" }),
        new ComparisonField({ title: "Minimum EMP radius", units: "m", valueFn: i => i.emp.minEmpRadius, sortDirection: "desc" }),
        new ComparisonField({ title: "Physical radius", units: "m", valueFn: i => i.emp.physRadius, sortDirection: "desc" }),
        new ComparisonField({ title: "Minimum physical radius", units: "m", valueFn: i => i.emp.minPhysRadius, sortDirection: "desc" }),
        new ComparisonField({ title: "Pressure", valueFn: i => i.emp.pressure, sortDirection: "desc" }),
        new ComparisonField({ title: "Unleash time", units: "s", valueFn: i => i.emp.unleashTime }),
        new ComparisonField({ title: "Cooldown time", units: "s", valueFn: i => i.emp.cooldownTime }),
      ]
    }),
    new ComparisonGroup({
      title: "Missle damage",
      visibleFn: items => !!_.find(items, i => !!i.missile),
      fields: [
        new ComparisonField({ title: "Physical damage", valueFn: i => i.missile.explosionParams.damage[0].DamagePhysical, sortDirection: "desc" }),
        new ComparisonField({ title: "Energy damage", valueFn: i => i.missile.explosionParams.damage[0].DamageEnergy, sortDirection: "desc" }),
        new ComparisonField({ title: "Distortion damage", valueFn: i => i.missile.explosionParams.damage[0].DamageDistortion, sortDirection: "desc" }),
        new ComparisonField({ title: "Thermal damage", valueFn: i => i.missile.explosionParams.damage[0].DamageThermal, sortDirection: "desc" }),
        new ComparisonField({ title: "Biochemical damage", valueFn: i => i.missile.explosionParams.damage[0].DamageBiochemical, sortDirection: "desc" }),
        new ComparisonField({ title: "Stun damage", valueFn: i => i.missile.explosionParams.damage[0].DamageStun, sortDirection: "desc" }),
      ]
    }),

    new ComparisonGroup({
      title: "Missile range",
      visibleFn: items => !!_.find(items, i => !!i.missile),
      fields: [
        new ComparisonField({ title: "Signal type", valueFn: i => i.missile.targetingParams.trackingSignalType }),
        new ComparisonField({ title: "Minimum tracking signal", valueFn: i => i.missile.targetingParams.trackingSignalMin }),
        new ComparisonField({ title: "Lock time", valueFn: i => i.missile.targetingParams.lockTime }),
        new ComparisonField({ title: "Maximum tracking distance", valueFn: i => i.missile.targetingParams.trackingDistanceMax, units: "m", sortDirection: "desc" }),
        new ComparisonField({ title: "Tracking angle", valueFn: i => i.missile.targetingParams.trackingAngle, units: "°", sortDirection: "desc" })
      ]
    }),

    new ComparisonGroup({
      title: "Mining laser",
      visibleFn: items => !!_.find(items, i => !!i.miningLaser),
      fields: [
        new ComparisonField({ title: "Slots", valueFn: i => i.itemPorts.length, sortDirection: "desc" }),
        new ComparisonField({ title: "Optimal range", units: "m", valueFn: i => i.weapon.fireActions.SWeaponActionFireBeamParams.fullDamageRange, sortDirection: "desc" }),
        new ComparisonField({ title: "Maximum range", units: "m", valueFn: i => i.weapon.fireActions.SWeaponActionFireBeamParams.zeroDamageRange, sortDirection: "desc" }),
        new ComparisonField({ title: "Power transfer", units: "W", valueFn: i => i.weapon.fireActions.SWeaponActionFireBeamParams.damagePerSecond.DamageInfo.DamageEnergy, sortDirection: "desc" }),
        new ComparisonField({ title: "Instability", units: "%", showPlus: true, valueFn: i => _.get(i, "miningLaser.miningLaserModifiers.laserInstability.FloatModifierMultiplicative.value", 0) }),
        new ComparisonField({ title: "Resistance", units: "%", showPlus: true, valueFn: i => _.get(i, "miningLaser.miningLaserModifiers.resistanceModifier") * 100 }),
        new ComparisonField({ title: "Throttle responsiveness", units: "%", showPlus: true, valueFn: i => _.get(i, "miningLaser.throttleLerpSpeed") }),
        new ComparisonField({ title: "Shatter damage", units: "%", showPlus: true, valueFn: i => _.get(i, "miningLaser.miningLaserModifiers.shatterdamageModifier.FloatModifierMultiplicative.value") }),
        new ComparisonField({ title: "Optimal charge window size", units: "%", showPlus: true, valueFn: i => _.get(i, "miningLaser.miningLaserModifiers.optimalChargeWindowSizeModifier.FloatModifierMultiplicative.value"), sortDirection: "desc" }),
        new ComparisonField({ title: "Catastrophic charge rate", units: "%", showPlus: true, valueFn: i => _.get(i, "miningLaser.miningLaserModifiers.catastrophicChargeWindowRateModifier.FloatModifierMultiplicative.value") }),
      ]
    }),
    new ComparisonGroup({
      title: "Ammunition",
      visibleFn: items => !!_.find(items, i => !!i.ammoContainer),
      fields: [
        new ComparisonField({ title: "Initial ammo count", valueFn: i => _.get(i, "ammoContainer.initialAmmoCount"), sortDirection: "desc" }),
        new ComparisonField({ title: "Max ammo count", valueFn: i => _.get(i, "ammoContainer.maxAmmoCount"), sortDirection: "desc" }),
      ]
    }),
    new ComparisonGroup({
      title: "Power plant",
      visibleFn: items => !!_.find(items, i => i.powerConnection && i.type == "PowerPlant"),
      fields: [
        new ComparisonField({ title: "Power generated", units: "W", valueFn: i => _.get(i, "powerConnection.PowerDraw"), sortDirection: "desc" }),
      ]
    }),
    new ComparisonGroup({
      title: "Power usage",
      visibleFn: items => !!_.find(items, i => i.powerConnection && i.type != "PowerPlant"),
      fields: [
        new ComparisonField({ title: "Standby power draw", units: "W", valueFn: i => _.get(i, "powerConnection.PowerBase") }),
        new ComparisonField({ title: "Full power draw", units: "W", valueFn: i => _.get(i, "powerConnection.PowerDraw") }),
      ]
    }),
    new ComparisonGroup({
      title: "Power emissions",
      visibleFn: items => !!_.find(items, i => i.powerConnection),
      fields: [
        new ComparisonField({ title: "Power to EM ratio", units: "J/W", decimals: 1, valueFn: i => _.get(i, "powerConnection.PowerToEM") }),
        new ComparisonField({ title: "EM at standby", units: "J", valueFn: i => _.get(i, "powerConnection.PowerBase") * _.get(i, "powerConnection.PowerToEM") }),
        new ComparisonField({ title: "EM at full power", units: "J", valueFn: i => _.get(i, "powerConnection.PowerDraw") * _.get(i, "powerConnection.PowerToEM") }),
      ]
    }),
    new ComparisonGroup({
      title: "Durability",
      visibleFn: items => !!_.find(items, i => i.degregation || i.health),
      fields: [
        new ComparisonField({ title: "Hitpoints", units: "HP", valueFn: i => i.health, sortDirection: "desc" }),
        new ComparisonField({ title: "Lifetime", units: "h", decimals: 1, valueFn: i => i.maxLifetime, sortDirection: "desc" }),
      ]
    }),
    new ComparisonGroup({
      title: "Thermal properties",
      visibleFn: items => !!_.find(items, i => i.heatConnection),
      collapsed: true,
      fields: [
        new ComparisonField({ title: "Min temperature", units: "C", valueFn: i => _.get(i, "heatConnection.MinTemperature") }),
        new ComparisonField({ title: "Max temperature", units: "C", valueFn: i => _.get(i, "heatConnection.MaxTemperature") }),
        new ComparisonField({ title: "Cooling starts at", units: "C", valueFn: i => _.get(i, "heatConnection.StartCoolingTemperature") }),
        new ComparisonField({ title: "Cooling rate", units: "W", valueFn: i => _.get(i, "heatConnection.MaxCoolingRate"), sortDirection: "desc" }),
        new ComparisonField({ title: "Misfires start at", units: "C", valueFn: i => _.get(i, "heatConnection.MisfireMinTemperature") }),
        new ComparisonField({ title: "Misfires peak at", units: "C", valueFn: i => _.get(i, "heatConnection.MisfireMaxTemperature") }),
        new ComparisonField({ title: "Standby thermal energy", units: "J", valueFn: i => _.get(i, "heatConnection.ThermalEnergyBase") }),
        new ComparisonField({ title: "Full thermal energy", units: "J", valueFn: i => _.get(i, "heatConnection.ThermalEnergyDraw") }),
        new ComparisonField({ title: "Specific heat capacity", units: "J/gC", decimals: 1, valueFn: i => _.get(i, "heatConnection.SpecificHeatCapacity") }),
        new ComparisonField({ title: "Mass", valueFn: i => _.get(i, "heatConnection.Mass") }),
        new ComparisonField({ title: "Thermal conductivity", decimals: 2, valueFn: i => _.get(i, "heatConnection.ThermalConductivity") }),
        new ComparisonField({ title: "Surface area", units: "m2", decimals: 2, valueFn: i => _.get(i, "heatConnection.SurfaceArea") }),
      ]
    }),
    new ComparisonGroup({
      title: "Thermal emissions",
      visibleFn: items => !!_.find(items, i => i.powerConnection),
      collapsed: true,
      fields: [
        new ComparisonField({ title: "Temperature to IR ratio", decimals: 1, valueFn: i => _.get(i, "heatConnection.TemperatureToIR") })
      ]
    })
  ];

  items: SCItem[] = [];
  compareAgainst: string = "";

  private currentSortField: ComparisonField<SCItem> = this.fields[0].fields[0];
  private currentSortDirection: "asc" | "desc" = "asc";

  constructor(private $http: HttpClient, private route: ActivatedRoute, private localisationSvc: LocalisationService) { }

  ngOnInit() {
    this.route.queryParamMap.subscribe(async params => {

      let itemsParam: string = params.get("items") || "";
      let itemClasses = itemsParam ? itemsParam.split(",") : await this.getItemNames(params.get("type") || "", params.get("subType") || "", parseInt(params.get("size") || "0"));

      this.compareAgainst = params.get("selected") || "";

      let itemPromises: Promise<SCItem>[] = [];

      this.items = [];
      for (let i = 0; i < itemClasses.length; i++) {
        itemPromises[i] = this.$http.get<any>(`${environment.api}/items/${itemClasses[i].toLowerCase()}.json`).toPromise().then(i => { this.items.push(new SCItem(i)); this.applySort(); return i; });
      }

      Promise.all(itemPromises).then(() => {
        this.fields.forEach(g => {
          g.fields.forEach(f => {
            if (g.visibleFn && g.visibleFn(this.items)) this.items.forEach(i => {
              let value = f.valueFn(i);
              if (value === undefined || typeof value !== "number") return;
              if (f.sortDirection == "asc" && (f.bestValue === undefined || value < f.bestValue)) f.bestValue = value;
              if (f.sortDirection == "asc" && (f.worstValue === undefined || value > f.worstValue)) f.worstValue = value;
              if (f.sortDirection == "desc" && (f.bestValue === undefined || value > f.bestValue)) f.bestValue = value;
              if (f.sortDirection == "desc" && (f.worstValue === undefined || value < f.worstValue)) f.worstValue = value;
            });
          });
        });
      });

    });
  }

  async getItemNames(type: string, subType: string, size: number): Promise<string[]> {
    let index = await this.$http.get<any>(`${environment.api}/items.json`).toPromise();

    return _.map(_.filter(index, (i: any) => (!size || i.size == size) && i.type == type && (!subType || (i.subType == subType))), x => x.className);
  }

  toggleCollapse(group: ComparisonGroup<SCItem>) {
    group.collapsed = !group.collapsed;
  }

  sortBy(field: ComparisonField<SCItem>) {
    if (field === this.currentSortField) this.currentSortDirection = this.currentSortDirection == "asc" ? "desc" : "asc";
    else this.currentSortDirection = field.sortDirection;
    this.currentSortField = field;
    this.applySort();
  }

  private applySort() {
    if (this.compareAgainst) this.items = _.orderBy(this.items, [i => this.compareAgainst == i.className, i => this.currentSortField.valueFn(i) || 0], ["desc", this.currentSortDirection]);
    else this.items = _.orderBy(this.items, [i => this.currentSortField.valueFn(i) || 0], [this.currentSortDirection]);
  }
}
