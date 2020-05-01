using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Vehicles
{
	public class Pipe
	{
		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public string @class;
	}
}
