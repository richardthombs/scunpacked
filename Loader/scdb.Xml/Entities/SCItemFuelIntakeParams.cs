using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SCItemFuelIntakeParams
	{
		[XmlAttribute]
		public double fuelPushRate;

		[XmlAttribute]
		public double minimumRate;
	}
}
