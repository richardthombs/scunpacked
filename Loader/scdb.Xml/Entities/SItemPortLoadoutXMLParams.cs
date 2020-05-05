using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SItemPortLoadoutXMLParams
	{
		[XmlAttribute]
		public string __type;

		[XmlAttribute]
		public string loadoutPath;
	}
}
