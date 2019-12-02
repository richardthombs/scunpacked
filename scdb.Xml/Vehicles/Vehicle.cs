using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class Vehicle
	{
		[XmlAttribute]
		public string name { get; set; }

		[XmlAttribute]
		public string displayname { get; set; }

		[XmlAttribute]
		public string subType { get; set; }

		[XmlAttribute]
		public int size { get; set; }

		[XmlAttribute]
		public string requiredItemTags { get; set; }

		[XmlAttribute]
		public string itemPortTags { get; set; }

		[XmlAttribute]
		public string crossSectionMultiplier { get; set; }

		public Camera[] Cameras { get; set; }

		public Pipe[] Pipes { get; set; }

		public Part[] Parts { get; set; }
		public MovementParams MovementParams { get; set; }
	}
}
