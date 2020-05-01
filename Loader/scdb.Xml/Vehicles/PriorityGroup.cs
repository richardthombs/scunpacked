using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Vehicles
{
	public class PriorityGroup
	{
		[XmlAttribute]
		public string itemType;

		[XmlAttribute]
		public string defaultPriority;
	}
}
