using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SItemDefinition
	{
		[XmlAttribute]
		public string DisplayType;

		[XmlAttribute]
		public int Grade;

		public SCItemLocalization Localization;

		[XmlAttribute]
		public string Manufacturer;

		[XmlAttribute]
		public string RequiredTags;

		[XmlAttribute]
		public int Size;

		[XmlAttribute]
		public string SubType;

		[XmlAttribute]
		public string Tags;

		[XmlAttribute]
		public string Type;
	}
}
