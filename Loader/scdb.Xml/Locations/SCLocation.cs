using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCLocation
	{
		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public string description;

		[XmlAttribute]
		public string type;

		[XmlAttribute]
		public string parent;

		[XmlAttribute]
		public string size;

		[XmlAttribute]
		public string __ref;

		[XmlAttribute]
		public string __path;
	}
}
