using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Vehicles
{
	public class Part
	{
		[XmlAttribute]
		public string @class;

		[XmlAttribute]
		public double damageMax;

		public ItemPort ItemPort;

		[XmlAttribute]
		public string mass; // Because 3.8.0 PTU has got dodgy data in the mass attribute

		[XmlAttribute]
		public string name;

		public Part[] Parts;

		[XmlAttribute]
		public bool skipPart;
	}
}
