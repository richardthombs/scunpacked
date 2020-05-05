using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Vehicles
{
	public class Type
	{
		[XmlAttribute]
		public string subtypes;

		[XmlAttribute]
		public string type;
	}
}
