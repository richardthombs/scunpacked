using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
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
