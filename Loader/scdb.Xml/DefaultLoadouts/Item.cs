using System;
using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.DefaultLoadouts
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
