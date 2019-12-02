using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemLocalization
	{
		[XmlAttribute]
		public string Name { get; set; }

		[XmlAttribute]
		public string Description { get; set; }
	}
}