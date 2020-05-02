using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SItemPortDefTypes
	{
		public Enum[] SubTypes;

		[XmlAttribute]
		public string Type;
	}

	public class Enum
	{
		[XmlAttribute]
		public string value;
	}
}
