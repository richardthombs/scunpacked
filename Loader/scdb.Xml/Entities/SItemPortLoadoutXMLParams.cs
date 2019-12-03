using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SItemPortLoadoutXMLParams
	{
		[XmlAttribute]
		public string loadoutPath;

		[XmlAttribute]
		public string __type;
	}
}