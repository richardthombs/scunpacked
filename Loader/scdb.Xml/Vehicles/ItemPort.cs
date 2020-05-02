using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Vehicles
{
	public class ItemPort
	{
		public Connection[] Connections;
		public ControllerDef ControllerDef;

		[XmlAttribute]
		public string defaultWeaponGroup;

		[XmlAttribute]
		public string display_name;

		[XmlAttribute]
		public string flags;

		[XmlAttribute]
		public string id;

		[XmlAttribute]
		public int maxsize;

		[XmlAttribute]
		public int minsize;

		public AngleRange Pitch;

		[XmlAttribute]
		public string requiredTags;

		public Type[] Types;
		public AngleRange Yaw;
	}
}
