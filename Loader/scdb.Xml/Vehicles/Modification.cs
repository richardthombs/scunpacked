using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Vehicles
{
	public class Modification
	{
		public Elem[] Elems;

		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public string patchFile;
	}

	public class Elem
	{
		[XmlAttribute]
		public string idRef;

		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public string value;
	}

	public class Modifications
	{
		public MovementParams MovementParams;

		public Part[] Parts;
	}
}
