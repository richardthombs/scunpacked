using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class AmmoParams
	{
		public string ClassName;

		[XmlAttribute]
		public int size;

		[XmlAttribute]
		public double lifetime;

		[XmlAttribute]
		public double speed;

		[XmlAttribute]
		public string __ref;

		public ProjectileParams projectileParams;
	}
}
