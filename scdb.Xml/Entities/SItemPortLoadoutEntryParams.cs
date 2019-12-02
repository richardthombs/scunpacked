using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SItemPortLoadoutEntryParams
	{
		[XmlAttribute]
		public string itemPortName { get; set; }

		[XmlAttribute]
		public string entityClassName { get; set; }
	}
}