using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class AngleRange
	{
		[XmlAttribute]
		public int min;

		[XmlAttribute]
		public int max;
	}
}
