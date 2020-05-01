using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Vehicles
{
	public class Modification
	{
		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public string patchFile;

		public Elem[] Elems;
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
		public Part[] Parts;
		public MovementParams MovementParams;
	}
}
