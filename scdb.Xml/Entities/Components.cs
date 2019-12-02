using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class Components
	{
		public VehicleComponentParams VehicleComponentParams { get; set; }
		public SEntityComponentDefaultLoadoutParams SEntityComponentDefaultLoadoutParams { get; set; }
		public SAttachableComponentParams SAttachableComponentParams { get; set; }
		public SHealthComponentParams SHealthComponentParams { get; set; }
		public SCItemVehicleArmorParams SCItemVehicleArmorParams { get; set; }
		public SCItemCargoGridParams SCItemCargoGridParams { get; set; }
		public IFCSParams IFCSParams { get; set; }
		public SCItemCoolerParams SCItemCoolerParams { get; set; }
		public SDegradationParams SDegradationParams { get; set; }
		public EntityComponentPowerConnection EntityComponentPowerConnection { get; set; }
		public EntityComponentHeatConnection EntityComponentHeatConnection { get; set; }
		public SDistortionParams SDistortionParams { get; set; }
		public SCItemFuelIntakeParams SCItemFuelIntakeParams { get; set; }
		public SCItem SCItem { get; set; }
		public SAmmoContainerComponentParams SAmmoContainerComponentParams { get; set; }
		public SCItemWeaponComponentParams SCItemWeaponComponentParams { get; set; }
		public SCItemFuelTankParams SCItemFuelTankParams { get; set; }
		public SCItemQuantumDriveParams SCItemQuantumDriveParams { get; set; }
		public SCItemQuantumInterdictionGeneratorParams SCItemQuantumInterdictionGeneratorParams { get; set; }
	}

	public class SCItemQuantumInterdictionGeneratorParams
	{
		public SCItemQuantumJammerParams[] jammerSettings { get; set; }
		public SCItemQuantumInterdictionPulseParams[] quantumInterdictionPulseSettings { get; set; }
	}

	public class SCItemQuantumJammerParams
	{
		[XmlAttribute]
		public double jammerRange { get; set; }

		[XmlAttribute]
		public double maxPowerDraw { get; set; }

		[XmlAttribute]
		public double greenZoneCheckRange { get; set; }

		[XmlAttribute]
		public string setJammerSwitchOn { get; set; }

		[XmlAttribute]
		public string setJammerSwitchOff { get; set; }
	}

	public class SCItemQuantumInterdictionPulseParams
	{
		[XmlAttribute]
		public double chargeTimeSecs { get; set; }

		[XmlAttribute]
		public double dischargeTimeSecs { get; set; }

		[XmlAttribute]
		public double cooldownTimeSecs { get; set; }

		[XmlAttribute]
		public double radiusMeters { get; set; }

		[XmlAttribute]
		public double decreaseChargeRateTimeSeconds { get; set; }

		[XmlAttribute]
		public double increaseChargeRateTimeSeconds { get; set; }

		[XmlAttribute]
		public double activationPhaseDuration_seconds { get; set; }

		[XmlAttribute]
		public double disperseChargeTimeSeconds { get; set; }

		[XmlAttribute]
		public double maxPowerDraw { get; set; }

		[XmlAttribute]
		public double stopChargingPowerDrawFraction { get; set; }

		[XmlAttribute]
		public double maxChargeRatePowerDrawFraction { get; set; }

		[XmlAttribute]
		public double activePowerDrawFraction { get; set; }

		[XmlAttribute]
		public double tetheringPowerDrawFraction { get; set; }

		[XmlAttribute]
		public double greenZoneCheckRange { get; set; }

		[XmlAttribute]
		public string startChargingIP { get; set; }

		[XmlAttribute]
		public string cancelChargingIP { get; set; }

		[XmlAttribute]
		public string disperseChargeIP { get; set; }
	}

	public class SCItemQuantumDriveParams
	{
		[XmlAttribute]
		public string tracePoint { get; set; }

		[XmlAttribute]
		public double quantumFuelRequirement { get; set; }

		[XmlAttribute]
		public double jumpRange { get; set; }

		[XmlAttribute]
		public double disconnectRange { get; set; }

		public SQuantumDriveParams @params { get; set; }
		public SQuantumDriveParams splineJumpParams { get; set; }
		public QuantumDriveHeatParams heatParams { get; set; }
	}

	public class QuantumDriveHeatParams
	{
		[XmlAttribute]
		public double preRampUpThermalEnergyDraw { get; set; }

		[XmlAttribute]
		public double rampUpThermalEnergyDraw { get; set; }

		[XmlAttribute]
		public double inFlightThermalEnergyDraw { get; set; }

		[XmlAttribute]
		public double rampDownThermalEnergyDraw { get; set; }

		[XmlAttribute]
		public double postRampDownThermalEnergyDraw { get; set; }
	}

	public class SQuantumDriveParams
	{
		[XmlAttribute]
		public double driveSpeed { get; set; }

		[XmlAttribute]
		public double cooldownTime { get; set; }

		[XmlAttribute]
		public double stageOneAccelRate { get; set; }

		[XmlAttribute]
		public double stageTwoAccelRate { get; set; }

		[XmlAttribute]
		public double engageSpeed { get; set; }

		[XmlAttribute]
		public double interdictionEffectTime { get; set; }

		[XmlAttribute]
		public double calibrationRate { get; set; }

		[XmlAttribute]
		public double minCalibrationRequirement { get; set; }

		[XmlAttribute]
		public double maxCalibrationRequirement { get; set; }

		[XmlAttribute]
		public double calibrationProcessAngleLimit { get; set; }

		[XmlAttribute]
		public double calibrationWarningAngleLimit { get; set; }

		[XmlAttribute]
		public double calibrationDelayInSeconds { get; set; }

		[XmlAttribute]
		public double spoolUpTime { get; set; }

		[XmlAttribute]
		public string turnOnSpoolInteraction { get; set; }

		[XmlAttribute]
		public string turnOffSpoolInteraction { get; set; }
	}

	public class SCItemFuelTankParams
	{
		[XmlAttribute]
		public double fillRate { get; set; }

		[XmlAttribute]
		public double drainRate { get; set; }

		[XmlAttribute]
		public double capacity { get; set; }
	}

	public class SCItemWeaponComponentParams
	{
		[XmlAttribute]
		public string ammoContainerRecord { get; set; }

		public connectionParams connectionParams { get; set; }
	}

	public class connectionParams
	{
		[XmlAttribute]
		public double powerActiveCooldown { get; set; }

		[XmlAttribute]
		public double heatRateOnline { get; set; }

		[XmlAttribute]
		public double maxGlow { get; set; }

		public SWeaponStats noPowerStats { get; set; }
		public SWeaponStats underpowerStats { get; set; }
		public SWeaponStats overpowerStats { get; set; }
		public SWeaponStats overclockStats { get; set; }
		public SWeaponStats heatStats { get; set; }
	}

	public class SWeaponStats
	{
		[XmlAttribute]
		public double fireRate { get; set; }

		[XmlAttribute]
		public double fireRateMultiplier { get; set; }

		[XmlAttribute]
		public double damageMultiplier { get; set; }

		[XmlAttribute]
		public double pellets { get; set; }

		[XmlAttribute]
		public double burstShots { get; set; }

		[XmlAttribute]
		public double ammoCost { get; set; }

		[XmlAttribute]
		public double ammoCostMultiplier { get; set; }

		[XmlAttribute]
		public double heatGenerationMultiplier { get; set; }

		[XmlAttribute]
		public double soundRadiusMultiplier { get; set; }

		[XmlAttribute]
		public bool useAlternateProjectileVisuals { get; set; }
	}

	public class SAmmoContainerComponentParams
	{
		[XmlAttribute]
		public double initialAmmoCount { get; set; }

		[XmlAttribute]
		public double maxAmmoCount { get; set; }

		[XmlAttribute]
		public string ammoParamsRecord { get; set; }
	}

	public class SCItem
	{
		[XmlAttribute]
		public bool turnedOnByDefault { get; set; }

		[XmlAttribute]
		public string turnOnInteraction { get; set; }

		[XmlAttribute]
		public string turnOffInteraction { get; set; }

		[XmlAttribute]
		public string placeInteraction { get; set; }

		[XmlAttribute]
		public string attachToTileItemPort { get; set; }

		[XmlAttribute]
		public bool inheritModelTagFromHost { get; set; }

		public SItemPortCoreParams[] ItemPorts { get; set; }
	}

	public class SItemPortCoreParams
	{
		[XmlAttribute]
		public string PortFlags { get; set; }

		[XmlAttribute]
		public string PortTags { get; set; }

		[XmlAttribute]
		public string RequiredItemTags { get; set; }

		public SItemPortDef[] Ports { get; set; }
	}

	public class SItemPortDef
	{
		[XmlAttribute]
		public string Name { get; set; }

		[XmlAttribute]
		public string DisplayName { get; set; }

		[XmlAttribute]
		public string PortTags { get; set; }

		[XmlAttribute]
		public string RequiredPortTags { get; set; }

		[XmlAttribute]
		public string Flags { get; set; }

		[XmlAttribute]
		public int MinSize { get; set; }

		[XmlAttribute]
		public int MaxSize { get; set; }

		[XmlAttribute]
		public int InteractionPointSize { get; set; }

		[XmlAttribute]
		public string DefaultWeaponGroup { get; set; }

		[XmlAttribute]
		public string controllableTags { get; set; }

		public SItemPortDefTypes[] Types { get; set; }
	}

	public class SItemPortDefTypes
	{
		[XmlAttribute]
		public string Type { get; set; }
	}

	public class SCItemFuelIntakeParams
	{
		[XmlAttribute]
		public double fuelPushRate { get; set; }

		[XmlAttribute]
		public double minimumRate { get; set; }
	}

	public class SDistortionParams
	{
		[XmlAttribute]
		public double DecayRate { get; set; }

		[XmlAttribute]
		public double Maximum { get; set; }

		[XmlAttribute]
		public double OverloadRatio { get; set; }

		[XmlAttribute]
		public double RecoveryRatio { get; set; }

		[XmlAttribute]
		public double RecoveryTime { get; set; }
	}

	public class EntityComponentHeatConnection
	{
		[XmlAttribute]
		public double TemperatureToIR { get; set; }

		[XmlAttribute]
		public double OverpowerHeat { get; set; }

		[XmlAttribute]
		public double OverclockThresholdMinHeat { get; set; }

		[XmlAttribute]
		public double OverclockThresholdMaxHeat { get; set; }

		[XmlAttribute]
		public double ThermalEnergyBase { get; set; }

		[XmlAttribute]
		public double ThermalEnergyDraw { get; set; }

		[XmlAttribute]
		public double ThermalConductivity { get; set; }

		[XmlAttribute]
		public double SpecificHeatCapacity { get; set; }

		[XmlAttribute]
		public double Mass { get; set; }

		[XmlAttribute]
		public double SurfaceArea { get; set; }

		[XmlAttribute]
		public double StartCoolingTemperature { get; set; }

		[XmlAttribute]
		public double MaxCoolingRate { get; set; }

		[XmlAttribute]
		public double MaxTemperature { get; set; }

		[XmlAttribute]
		public double OverheatTemperature { get; set; }

		[XmlAttribute]
		public double RecoveryTemperature { get; set; }

		[XmlAttribute]
		public double MinTemperature { get; set; }

		[XmlAttribute]
		public double MisfireMinTemperature { get; set; }

		[XmlAttribute]
		public double MisfireMaxTemperature { get; set; }
	}

	public class EntityComponentPowerConnection
	{
		[XmlAttribute]
		public double PowerBase { get; set; }

		[XmlAttribute]
		public double PowerDraw { get; set; }

		[XmlAttribute]
		public double TimeToReachDrawRequest { get; set; }

		[XmlAttribute]
		public double SafeguardPriority { get; set; }

		[XmlAttribute]
		public bool DisplayedInPoweredItemList { get; set; }

		[XmlAttribute]
		public bool IsThrottleable { get; set; }

		[XmlAttribute]
		public bool IsOverclockable { get; set; }

		[XmlAttribute]
		public double OverclockThresholdMin { get; set; }

		[XmlAttribute]
		public double OverclockThresholdMax { get; set; }

		[XmlAttribute]
		public double OverpowerPerformance { get; set; }

		[XmlAttribute]
		public double OverclockPerformance { get; set; }

		[XmlAttribute]
		public double PowerToEM { get; set; }

		[XmlAttribute]
		public double DecayRateOfEM { get; set; }

		[XmlAttribute]
		public double WarningDelayTime { get; set; }

		[XmlAttribute]
		public double WarningDisplayTime { get; set; }
	}

	public class SDegradationParams
	{
		[XmlAttribute]
		public double MaxLifetimeHours { get; set; }

		[XmlAttribute]
		public double InitialAgeRatio { get; set; }

		[XmlAttribute]
		public bool DegradeOnlyWhenAttached { get; set; }
	}

	public class SCItemCoolerParams
	{
		[XmlAttribute]
		public double CoolingRate { get; set; }

		[XmlAttribute]
		public double SuppressionIRFactor { get; set; }

		[XmlAttribute]
		public double SuppressionHeatFactor { get; set; }
	}

	public class IFCSParams
	{
		[XmlAttribute]
		public double maxSpeed { get; set; }

		[XmlAttribute]
		public double maxAfterburnSpeed { get; set; }

		[XmlAttribute]
		public double torqueDistanceThreshold { get; set; }

		[XmlAttribute]
		public bool refreshCachesOnLandingMode { get; set; }

		[XmlAttribute]
		public double coefficientOfLift { get; set; }

		[XmlAttribute]
		public double centerOfLiftOffset { get; set; }

		[XmlAttribute]
		public double centerOfPressureOffset { get; set; }

		[XmlAttribute]
		public double linearTurbulenceAmplitude { get; set; }

		[XmlAttribute]
		public double angularTurbulenceAmplitude { get; set; }

		[XmlAttribute]
		public double groundTurbulenceAmplitude { get; set; }

		[XmlAttribute]
		public double precisionMinDistance { get; set; }

		[XmlAttribute]
		public double precisionMaxDistance { get; set; }

		[XmlAttribute]
		public double precisionLandingMultiplier { get; set; }
	}

	public class SCItemCargoGridParams
	{
		public Vec3 dimensions { get; set; }
	}

	public class Vec3
	{
		[XmlAttribute]
		public double x { get; set; }

		[XmlAttribute]
		public double y { get; set; }

		[XmlAttribute]
		public double z { get; set; }
	}

	public class SCItemVehicleArmorParams
	{
		[XmlAttribute]
		public double signalInfrared { get; set; }

		[XmlAttribute]
		public double signalElectromagnetic { get; set; }

		[XmlAttribute]
		public double signalCrossSection { get; set; }

		public DamageMultiplier damageMultiplier { get; set; }
	}

	public class DamageMultiplier
	{
		public DamageInfo DamageInfo { get; set; }
	}

	public class DamageInfo
	{

		[XmlAttribute]
		public double DamagePhysical { get; set; }

		[XmlAttribute]
		public double DamageEnergy { get; set; }

		[XmlAttribute]
		public double DamageDistortion { get; set; }

		[XmlAttribute]
		public double DamageThermal { get; set; }

		[XmlAttribute]
		public double DamageBiochemical { get; set; }

		[XmlAttribute]
		public double DamageStun { get; set; }
	}
}