using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class Vec3
	{
		[XmlAttribute]
		public double x;

		[XmlAttribute]
		public double y;

		[XmlAttribute]
		public double z;
	}
}