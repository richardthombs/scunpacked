using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SCItemThrusterParams
	{
		[XmlAttribute]
		public double fuelBurnRatePer10KNewton;

		[XmlAttribute]
		public double minHealthThrustMultiplier;

		[XmlAttribute]
		public double thrustCapacity;

		[XmlAttribute]
		public string thrusterType;
	}
}
