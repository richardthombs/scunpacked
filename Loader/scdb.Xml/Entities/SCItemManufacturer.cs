using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemManufacturer
	{
		[XmlAttribute]
		public string Code;

		[XmlAttribute]
		public string __ref;

		public SCItemLocalization Localization;
	}
}
