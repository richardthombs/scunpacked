using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class Vehicle
	{
		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public string displayname;

		[XmlAttribute]
		public string subType;

		[XmlAttribute]
		public int size;

		[XmlAttribute]
		public string requiredItemTags;

		[XmlAttribute]
		public string itemPortTags;

		[XmlAttribute]
		public string crossSectionMultiplier;

		public Camera[] Cameras;

		public Pipe[] Pipes;

		public Part[] Parts;
		public MovementParams MovementParams;

		public Modification[] Modifications;
	}
}
