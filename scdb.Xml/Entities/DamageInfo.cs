using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class DamageInfo
	{

		[XmlAttribute]
		public double DamagePhysical;

		[XmlAttribute]
		public double DamageEnergy;

		[XmlAttribute]
		public double DamageDistortion;

		[XmlAttribute]
		public double DamageThermal;

		[XmlAttribute]
		public double DamageBiochemical;

		[XmlAttribute]
		public double DamageStun;
	}
}