using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class Pipe
	{
		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public string @class;
	}
}
