using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class ExplosionParams
	{
		public DamageInfo[] damage;

		[XmlAttribute]
		public double effectScale;

		[XmlAttribute]
		public double effectScaleMax;

		[XmlAttribute]
		public double effectScaleMin;

		[XmlAttribute]
		public string friendlyFire;

		[XmlAttribute]
		public string hitType;

		[XmlAttribute]
		public double holeSize;

		[XmlAttribute]
		public double maxblurdist;

		[XmlAttribute]
		public double maxPhysRadius;

		[XmlAttribute]
		public double maxRadius;

		[XmlAttribute]
		public double minPhysRadius;

		[XmlAttribute]
		public double minRadius;

		[XmlAttribute]
		public double pressure;

		[XmlAttribute]
		public double soundRadius;

		[XmlAttribute]
		public double terrainHoleSize;

		[XmlAttribute]
		public double useRandomScale;
	}
}
