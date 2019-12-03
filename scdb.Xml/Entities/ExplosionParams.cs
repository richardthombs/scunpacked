using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class ExplosionParams
	{
		[XmlAttribute]
		public string friendlyFire;

		[XmlAttribute]
		public double minRadius;

		[XmlAttribute]
		public double maxRadius;

		[XmlAttribute]
		public double soundRadius;

		[XmlAttribute]
		public double minPhysRadius;

		[XmlAttribute]
		public double maxPhysRadius;

		[XmlAttribute]
		public double pressure;

		[XmlAttribute]
		public double holeSize;

		[XmlAttribute]
		public double terrainHoleSize;

		[XmlAttribute]
		public double maxblurdist;

		[XmlAttribute]
		public double effectScale;

		[XmlAttribute]
		public double useRandomScale;

		[XmlAttribute]
		public double effectScaleMin;

		[XmlAttribute]
		public double effectScaleMax;

		[XmlAttribute]
		public string hitType;

		public DamageInfo[] damage;
	}
}