using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemLocalization
	{
		[XmlAttribute]
		public string Name;

		[XmlAttribute]
		public string Description;
	}
}