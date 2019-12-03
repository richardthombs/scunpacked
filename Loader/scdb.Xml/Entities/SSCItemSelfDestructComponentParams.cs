using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SSCItemSelfDestructComponentParams
	{
		[XmlAttribute]
		public double damage;

		[XmlAttribute]
		public double minRadius;

		[XmlAttribute]
		public double radius;

		[XmlAttribute]
		public double minPhysRadius;

		[XmlAttribute]
		public double physRadius;

		[XmlAttribute]
		public double time;
	}
}