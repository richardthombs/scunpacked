using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public abstract class Component
	{
		[XmlAttribute]
		public string __type { get; set; }
	}
}