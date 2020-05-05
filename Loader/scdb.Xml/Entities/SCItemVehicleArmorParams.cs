using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SCItemVehicleArmorParams
	{
		public DamageMultiplier damageMultiplier;

		[XmlAttribute]
		public double signalCrossSection;

		[XmlAttribute]
		public double signalElectromagnetic;

		[XmlAttribute]
		public double signalInfrared;
	}
}
