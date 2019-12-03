using System;
using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class Part
	{
		[XmlAttribute]
		public string name;

		[XmlAttribute]
		public double mass;

		[XmlAttribute]
		public double damageMax;

		[XmlAttribute]
		public string @class;

		public Part[] Parts;

		public ItemPort ItemPort;
	}
}
