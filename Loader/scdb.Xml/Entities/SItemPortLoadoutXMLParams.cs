using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SItemPortLoadoutXMLParams
	{
		[XmlAttribute]
		public string loadoutPath;

		[XmlAttribute]
		public string __type;
	}
}