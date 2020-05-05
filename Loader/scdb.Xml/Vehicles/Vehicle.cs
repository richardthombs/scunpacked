using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Vehicles
{
	public class Vehicle
	{
		public Camera[] Cameras;

		[XmlAttribute]
		public string crossSectionMultiplier;

		[XmlAttribute]
		public string displayname;

		[XmlAttribute]
		public string itemPortTags;

		public Modification[] Modifications;

		public MovementParams MovementParams;

		[XmlAttribute]
		public string name;

		public Part[] Parts;

		public Pipe[] Pipes;

		[XmlAttribute]
		public string requiredItemTags;

		[XmlAttribute]
		public int size;

		[XmlAttribute]
		public string subType;
	}
}
