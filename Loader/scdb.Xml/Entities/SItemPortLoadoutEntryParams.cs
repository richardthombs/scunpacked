using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
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