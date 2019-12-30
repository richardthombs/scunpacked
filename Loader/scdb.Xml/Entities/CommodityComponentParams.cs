using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class CommodityComponentParams
	{
		[XmlAttribute]
		public string type;

		[XmlAttribute]
		public string subtype;

		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public string description;

		[XmlAttribute]
		public bool IsUnrefinedElement;
	}
}
