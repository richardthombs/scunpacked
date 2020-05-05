using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SCItemMissileParams
	{
		public ExplosionParams explosionParams;

		[XmlAttribute]
		public double igniteTime;

		[XmlAttribute]
		public double irSignalDecayRate;

		[XmlAttribute]
		public double irSignalMaxValue;

		[XmlAttribute]
		public double irSignalMinValue;

		[XmlAttribute]
		public double irSignalRiseRate;

		[XmlAttribute]
		public double projectileProximity;

		[XmlAttribute]
		public double safeDistance;

		public STargetingParams targetingParams;
	}
}
