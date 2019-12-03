using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class Type
	{
		[XmlAttribute]
		public string type;

		[XmlAttribute]
		public string subtypes;
	}
}
