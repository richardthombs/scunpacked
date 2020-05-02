using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SCItemQuantumInterdictionPulseParams
	{
		[XmlAttribute]
		public double activationPhaseDuration_seconds;

		[XmlAttribute]
		public double activePowerDrawFraction;

		[XmlAttribute]
		public string cancelChargingIP;

		[XmlAttribute]
		public double chargeTimeSecs;

		[XmlAttribute]
		public double cooldownTimeSecs;

		[XmlAttribute]
		public double decreaseChargeRateTimeSeconds;

		[XmlAttribute]
		public double dischargeTimeSecs;

		[XmlAttribute]
		public string disperseChargeIP;

		[XmlAttribute]
		public double disperseChargeTimeSeconds;

		[XmlAttribute]
		public double greenZoneCheckRange;

		[XmlAttribute]
		public double increaseChargeRateTimeSeconds;

		[XmlAttribute]
		public double maxChargeRatePowerDrawFraction;

		[XmlAttribute]
		public double maxPowerDraw;

		[XmlAttribute]
		public double radiusMeters;

		[XmlAttribute]
		public string startChargingIP;

		[XmlAttribute]
		public double stopChargingPowerDrawFraction;

		[XmlAttribute]
		public double tetheringPowerDrawFraction;
	}
}
