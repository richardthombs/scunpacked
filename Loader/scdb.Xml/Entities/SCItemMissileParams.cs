using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemMissileParams
	{
		[XmlAttribute]
		public double safeDistance;

		[XmlAttribute]
		public double igniteTime;

		[XmlAttribute]
		public double irSignalMinValue;

		[XmlAttribute]
		public double irSignalMaxValue;

		[XmlAttribute]
		public double irSignalRiseRate;

		[XmlAttribute]
		public double irSignalDecayRate;

		[XmlAttribute]
		public double projectileProximity;

		public STargetingParams targetingParams;
		public ExplosionParams explosionParams;
	}
}