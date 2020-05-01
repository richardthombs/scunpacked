using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Vehicles
{
	public class Connection
	{
		[XmlAttribute]
		public string pipeClass;

		[XmlAttribute]
		public string pipe;
	}
}
