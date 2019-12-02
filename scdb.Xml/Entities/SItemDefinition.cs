using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SItemDefinition
	{
		[XmlAttribute]
		public string Type { get; set; }

		[XmlAttribute]
		public string SubType { get; set; }

		[XmlAttribute]
		public int Size { get; set; }

		[XmlAttribute]
		public int Grade { get; set; }

		[XmlAttribute]
		public string Manufacturer { get; set; }

		[XmlAttribute]
		public string Tags;

		[XmlAttribute]
		public string __type { get; set; }

		public SCItemLocalization Localization { get; set; }
	}
}