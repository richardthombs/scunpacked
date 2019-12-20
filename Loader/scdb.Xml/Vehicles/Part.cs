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
		public string mass; // Because 3.8.0 PTU has got dodgy data in the mass attribute

		[XmlAttribute]
		public double damageMax;

		[XmlAttribute]
		public string @class;

		[XmlAttribute]
		public bool skipPart;

		public Part[] Parts;

		public ItemPort ItemPort;
	}
}
