using System;
using System.Xml;
using System.Xml.Serialization;

namespace shipparser
{
	// A separate XML loadout file starts with a <Loadout> element and contains an <Items> element with an array of <Item>s, each of which can also have an <Items> array.
	// Each Item has a portName and an itemName, which seem to correspond to the itemPortName and entityClassName of hardcoded SItemPortLoadoutEntryParams.
	public class Loadout
	{
		public Item[] Items { get; set; }
	}

	public class Item
	{
		[XmlAttribute]
		public string portName { get; set; }

		[XmlAttribute]
		public string itemName { get; set; }

		public Item[] Items { get; set; }
	}
}