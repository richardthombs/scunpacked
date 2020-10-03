using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class StarMapObject
	{
		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public string description;

		[XmlAttribute]
		public string callout1;

		[XmlAttribute]
		public string callout2;

		[XmlAttribute]
		public string callout3;

		[XmlAttribute]
		public string type;

		[XmlAttribute]
		public string navIcon;

		[XmlAttribute]
		public string hideInStarmap;

		[XmlAttribute]
		public string jurisdiction;

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
