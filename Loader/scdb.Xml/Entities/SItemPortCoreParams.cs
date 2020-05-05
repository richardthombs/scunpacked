using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SItemPortCoreParams
	{
		[XmlAttribute]
		public string PortFlags;

		public SItemPortDef[] Ports;

		[XmlAttribute]
		public string PortTags;

		[XmlAttribute]
		public string RequiredItemTags;
	}
}
