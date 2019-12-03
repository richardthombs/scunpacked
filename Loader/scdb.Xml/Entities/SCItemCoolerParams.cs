using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemCoolerParams
	{
		[XmlAttribute]
		public double CoolingRate;

		[XmlAttribute]
		public double SuppressionIRFactor;

		[XmlAttribute]
		public double SuppressionHeatFactor;
	}
}