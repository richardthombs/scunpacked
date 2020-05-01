using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Vehicles
{
	public class Type
	{
		[XmlAttribute]
		public string type;

		[XmlAttribute]
		public string subtypes;
	}
}
