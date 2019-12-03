using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemFuelIntakeParams
	{
		[XmlAttribute]
		public double fuelPushRate;

		[XmlAttribute]
		public double minimumRate;
	}
}