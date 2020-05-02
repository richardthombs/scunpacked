using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Vehicles
{
	public class Connection
	{
		[XmlAttribute]
		public string pipe;

		[XmlAttribute]
		public string pipeClass;
	}
}
