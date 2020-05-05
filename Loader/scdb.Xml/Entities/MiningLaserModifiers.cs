using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class MiningLaserModifiers
	{
		public MiningLaserModifier catastrophicChargeWindowRateModifier;

		public MiningLaserModifier laserInstability;

		public MiningLaserModifier optimalChargeWindowRateModifier;

		public MiningLaserModifier optimalChargeWindowSizeModifier;

		[XmlAttribute]
		public double resistanceModifier;

		public MiningLaserModifier shatterdamageModifier;
	}
}
