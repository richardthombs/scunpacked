using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class CommodityComponentParams
	{
		[XmlAttribute]
		public string description;

		[XmlAttribute]
		public bool IsUnrefinedElement;

		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public string subtype;

		[XmlAttribute]
		public string type;
	}
}
