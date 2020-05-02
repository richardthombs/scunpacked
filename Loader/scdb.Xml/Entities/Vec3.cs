using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
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
