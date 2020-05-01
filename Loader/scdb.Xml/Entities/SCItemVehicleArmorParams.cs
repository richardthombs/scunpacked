using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SCItemVehicleArmorParams
	{
		[XmlAttribute]
		public double signalInfrared;

		[XmlAttribute]
		public double signalElectromagnetic;

		[XmlAttribute]
		public double signalCrossSection;

		public DamageMultiplier damageMultiplier;
	}
}