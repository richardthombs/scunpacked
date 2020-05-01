using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SWeaponConnectionParams
	{
		[XmlAttribute]
		public double powerActiveCooldown;

		[XmlAttribute]
		public double heatRateOnline;

		[XmlAttribute]
		public double maxGlow;

		public SWeaponStats noPowerStats;
		public SWeaponStats underpowerStats;
		public SWeaponStats overpowerStats;
		public SWeaponStats overclockStats;
		public SWeaponStats heatStats;
	}
}