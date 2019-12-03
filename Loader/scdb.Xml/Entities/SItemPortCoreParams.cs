using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SItemPortCoreParams
	{
		[XmlAttribute]
		public string PortFlags;

		[XmlAttribute]
		public string PortTags;

		[XmlAttribute]
		public string RequiredItemTags;

		public SItemPortDef[] Ports;
	}
}