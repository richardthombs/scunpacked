using System;
using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class Part
	{
		[XmlAttribute]
		public string name { get; set; }

		[XmlAttribute]
		public double mass { get; set; }

		[XmlAttribute]
		public double damageMax { get; set; }

		[XmlAttribute]
		public string @class { get; set; }

		public Part[] Parts { get; set; }

		public ItemPort ItemPort { get; set; }
	}
}
