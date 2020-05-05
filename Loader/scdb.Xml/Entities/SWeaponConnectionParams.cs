using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SWeaponConnectionParams
	{
		[XmlAttribute]
		public double heatRateOnline;

		public SWeaponStats heatStats;

		[XmlAttribute]
		public double maxGlow;

		public SWeaponStats noPowerStats;

		public SWeaponStats overclockStats;

		public SWeaponStats overpowerStats;

		[XmlAttribute]
		public double powerActiveCooldown;

		public SWeaponStats underpowerStats;
	}
}
