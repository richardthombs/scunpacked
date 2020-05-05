using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Vehicles
{
	public class Pipe
	{
		[XmlAttribute]
		public string @class;

		[XmlAttribute]
		public string name;
	}
}
