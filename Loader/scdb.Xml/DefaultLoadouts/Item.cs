using System.Xml.Serialization;

namespace Loader.SCDb.Xml.DefaultLoadouts
{
	public class Item
	{
		[XmlAttribute]
		public string portName;

		[XmlAttribute]
		public string itemName;

		public Item[] Items;
	}
}
