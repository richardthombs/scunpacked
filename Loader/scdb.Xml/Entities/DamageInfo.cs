using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class DamageInfo
	{
		[XmlAttribute]
		public double DamageBiochemical;

		[XmlAttribute]
		public double DamageDistortion;

		[XmlAttribute]
		public double DamageEnergy;

		[XmlAttribute]
		public double DamagePhysical;

		[XmlAttribute]
		public double DamageStun;

		[XmlAttribute]
		public double DamageThermal;
	}
}
