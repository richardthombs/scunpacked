using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class ControllerDef
	{
		[XmlAttribute]
		public string controllableTags;

		public UsableDef UsableDef;
	}
}
