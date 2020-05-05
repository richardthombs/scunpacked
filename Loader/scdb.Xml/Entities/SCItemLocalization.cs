using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SCItemLocalization
	{
		[XmlAttribute]
		public string Description;

		[XmlAttribute]
		public string Name;
	}
}
