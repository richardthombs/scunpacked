using System;
using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Turbulent
{
	public class TurbulentEntry
	{
		[XmlAttribute]
		public string turbulentName { get; set; }

		[XmlAttribute]
		public string itemClass { get; set; }
	}
}