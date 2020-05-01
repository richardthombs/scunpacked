using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class MiningLaserModifiers
	{
		[XmlAttribute]
		public double resistanceModifier;

		public MiningLaserModifier laserInstability;
		public MiningLaserModifier optimalChargeWindowSizeModifier;
		public MiningLaserModifier shatterdamageModifier;
		public MiningLaserModifier optimalChargeWindowRateModifier;
		public MiningLaserModifier catastrophicChargeWindowRateModifier;
	}
}
