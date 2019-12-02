using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class AngleRange
	{
		[XmlAttribute]
		public int min { get; set; }

		[XmlAttribute]
		public int max { get; set; }
	}
}
