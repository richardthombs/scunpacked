using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class EntityClassDefinition
	{
		[XmlAttribute]
		public string __type { get; set; }

		public Components Components { get; set; }
	}
}