using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class PriorityGroup
	{
		[XmlAttribute]
		public string itemType { get; set; }

		[XmlAttribute]
		public string defaultPriority { get; set; }
	}
}
