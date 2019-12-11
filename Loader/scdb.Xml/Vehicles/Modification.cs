using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class Modification
	{
		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public string patchFile;
	}

	public class Modifications
	{
		public Part[] Parts;
		public MovementParams MovementParams;
	}
}
