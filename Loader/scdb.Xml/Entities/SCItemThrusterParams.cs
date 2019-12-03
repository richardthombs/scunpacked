using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemThrusterParams
	{
		[XmlAttribute]
		public double thrustCapacity;

		[XmlAttribute]
		public double minHealthThrustMultiplier;

		[XmlAttribute]
		public double fuelBurnRatePer10KNewton;

		[XmlAttribute]
		public string thrusterType;
	}
}