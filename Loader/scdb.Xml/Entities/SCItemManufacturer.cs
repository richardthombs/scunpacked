using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SCItemManufacturer
	{
		[XmlAttribute]
		public string __ref;

		[XmlAttribute]
		public string Code;

		public SCItemLocalization Localization;
	}
}
