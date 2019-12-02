using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class EntityClassDefinition
	{
		public Components Components { get; set; }

		public string ClassName { get; set; }
	}
}