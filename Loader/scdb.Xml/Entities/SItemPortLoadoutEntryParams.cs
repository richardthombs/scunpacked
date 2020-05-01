using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SItemPortLoadoutEntryParams
	{
		[XmlAttribute]
		public string itemPortName;

		[XmlAttribute]
		public string entityClassName;

		public loadout loadout;
	}
}