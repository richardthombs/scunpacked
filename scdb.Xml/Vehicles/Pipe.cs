using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class Pipe
	{
		[XmlAttribute]
		public string name { get; set; }

		[XmlAttribute]
		public string @class { get; set; }
	}
}
