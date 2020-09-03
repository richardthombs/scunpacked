using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class AmmoParams : ClassBase
	{
		[XmlAttribute]
		public int size;

		[XmlAttribute]
		public double lifetime;

		[XmlAttribute]
		public double speed;

		public ProjectileParams projectileParams;
	}
}
