using System.Xml.Serialization;

namespace scdb.Xml.Entities
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
