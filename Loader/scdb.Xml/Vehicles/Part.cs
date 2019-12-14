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
		// TODO: This should be a double but the 3.8.0 ptu seems to be full of garbage characters
		public string mass;

		[XmlAttribute]
		public double damageMax;

		[XmlAttribute]
		public string @class;

		public Part[] Parts;

		public ItemPort ItemPort;
	}
}
