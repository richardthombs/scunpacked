using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItem
	{
		[XmlAttribute]
		public bool turnedOnByDefault;

		[XmlAttribute]
		public string turnOnInteraction;

		[XmlAttribute]
		public string turnOffInteraction;

		[XmlAttribute]
		public string placeInteraction;

		[XmlAttribute]
		public string attachToTileItemPort;

		[XmlAttribute]
		public bool inheritModelTagFromHost;
	}
}
