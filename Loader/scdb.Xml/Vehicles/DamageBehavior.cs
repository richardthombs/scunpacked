using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Vehicles
{
	public class DamageBehavior
	{
		[XmlAttribute]
		public string @class;

		[XmlAttribute]
		public double damageRatioMin;

		public Part[] DetachPart;

		public DamageGroup Group;
	}

	public class DamageGroup
	{
		[XmlAttribute]
		public string name;
	}
}
