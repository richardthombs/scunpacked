using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SItemDefinition
	{
		[XmlAttribute]
		public string Type;

		[XmlAttribute]
		public string SubType;

		[XmlAttribute]
		public int Size;

		[XmlAttribute]
		public int Grade;

		[XmlAttribute]
		public string Manufacturer;

		[XmlAttribute]
		public string Tags;

		[XmlAttribute]
		public string RequiredTags;

		[XmlAttribute]
		public string DisplayType;

		public SCItemLocalization Localization;
	}
}
