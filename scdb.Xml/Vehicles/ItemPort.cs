using System;
using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class ItemPort
	{
		[XmlAttribute]
		public int minsize { get; set; }

		[XmlAttribute]
		public int maxsize { get; set; }

		[XmlAttribute]
		public string display_name { get; set; }

		[XmlAttribute]
		public string flags { get; set; }

		[XmlAttribute]
		public string defaultWeaponGroup { get; set; }

		[XmlAttribute]
		public string id { get; set; }

		[XmlAttribute]
		public string requiredTags { get; set; }

		public Type[] Types { get; set; }
		public Connection[] Connections { get; set; }
		public ControllerDef ControllerDef { get; set; }
		public AngleRange Pitch { get; set; }
		public AngleRange Yaw { get; set; }
	}
}
