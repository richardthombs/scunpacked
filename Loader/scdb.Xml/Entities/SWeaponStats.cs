using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SWeaponStats
	{
		[XmlAttribute]
		public double ammoCost;

		[XmlAttribute]
		public double ammoCostMultiplier;

		[XmlAttribute]
		public double burstShots;

		[XmlAttribute]
		public double damageMultiplier;

		[XmlAttribute]
		public double fireRate;

		[XmlAttribute]
		public double fireRateMultiplier;

		[XmlAttribute]
		public double heatGenerationMultiplier;

		[XmlAttribute]
		public double pellets;

		[XmlAttribute]
		public double soundRadiusMultiplier;

		[XmlAttribute]
		public bool useAlternateProjectileVisuals;
	}
}
