using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class ClassBase
	{
		public string ClassName;

		[XmlAttribute]
		public string __ref;

		[XmlAttribute]
		public string __type;
	}
}
