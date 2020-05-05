using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SItemPortLoadoutEntryParams
	{
		[XmlAttribute]
		public string entityClassName;

		[XmlAttribute]
		public string itemPortName;

		public loadout loadout;
	}
}
