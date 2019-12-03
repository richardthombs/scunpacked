using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemQuantumInterdictionPulseParams
	{
		[XmlAttribute]
		public double chargeTimeSecs;

		[XmlAttribute]
		public double dischargeTimeSecs;

		[XmlAttribute]
		public double cooldownTimeSecs;

		[XmlAttribute]
		public double radiusMeters;

		[XmlAttribute]
		public double decreaseChargeRateTimeSeconds;

		[XmlAttribute]
		public double increaseChargeRateTimeSeconds;

		[XmlAttribute]
		public double activationPhaseDuration_seconds;

		[XmlAttribute]
		public double disperseChargeTimeSeconds;

		[XmlAttribute]
		public double maxPowerDraw;

		[XmlAttribute]
		public double stopChargingPowerDrawFraction;

		[XmlAttribute]
		public double maxChargeRatePowerDrawFraction;

		[XmlAttribute]
		public double activePowerDrawFraction;

		[XmlAttribute]
		public double tetheringPowerDrawFraction;

		[XmlAttribute]
		public double greenZoneCheckRange;

		[XmlAttribute]
		public string startChargingIP;

		[XmlAttribute]
		public string cancelChargingIP;

		[XmlAttribute]
		public string disperseChargeIP;
	}
}