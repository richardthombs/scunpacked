using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Vehicles
{
	public class PriorityGroup
	{
		[XmlAttribute]
		public string defaultPriority;

		[XmlAttribute]
		public string itemType;
	}
}
