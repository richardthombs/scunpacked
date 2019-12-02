using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SItemPortLoadoutXMLParams
	{
		[XmlAttribute]
		public string loadoutPath { get; set; }

		[XmlAttribute]
		public string __type { get; set; }
	}
}