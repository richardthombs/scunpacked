using System;
using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class ItemPort
	{
		[XmlAttribute]
		public int minsize;

		[XmlAttribute]
		public int maxsize;

		[XmlAttribute]
		public string display_name;

		[XmlAttribute]
		public string flags;

		[XmlAttribute]
		public string defaultWeaponGroup;

		[XmlAttribute]
		public string id;

		[XmlAttribute]
		public string requiredTags;

		public Type[] Types;
		public Connection[] Connections;
		public ControllerDef ControllerDef;
		public AngleRange Pitch;
		public AngleRange Yaw;
	}
}
