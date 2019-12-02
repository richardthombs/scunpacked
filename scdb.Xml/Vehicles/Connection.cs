using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class Connection
	{
		[XmlAttribute]
		public string pipeClass { get; set; }

		[XmlAttribute]
		public string pipe { get; set; }
	}
}
