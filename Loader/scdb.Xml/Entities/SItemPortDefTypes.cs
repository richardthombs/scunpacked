using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SItemPortDefTypes
	{
		[XmlAttribute]
		public string Type;

		public Enum[] SubTypes;
	}

	public class Enum
	{
		[XmlAttribute]
		public string value;
	}
}