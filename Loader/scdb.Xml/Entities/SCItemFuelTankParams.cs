using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SCItemFuelTankParams
	{
		[XmlAttribute]
		public double fillRate;

		[XmlAttribute]
		public double drainRate;

		[XmlAttribute]
		public double capacity;
	}
}
