using System;
using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Loadouts
{
	public class Item
	{
		[XmlAttribute]
		public string portName { get; set; }

		[XmlAttribute]
		public string itemName { get; set; }

		public Item[] Items { get; set; }
	}
}
