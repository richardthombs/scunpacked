using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Vehicles
{
	public class ControllerDef
	{
		[XmlAttribute]
		public string controllableTags;

		public UsableDef UsableDef;
	}
}
