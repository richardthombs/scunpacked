using System.Xml.Serialization;

namespace Loader.SCDb.Xml.DefaultLoadouts
{
	public class Item
	{
		[XmlAttribute]
		public string itemName;

		public Item[] Items;

		[XmlAttribute]
		public string portName;
	}
}
