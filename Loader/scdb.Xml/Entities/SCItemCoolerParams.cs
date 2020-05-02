using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SCItemCoolerParams
	{
		[XmlAttribute]
		public double CoolingRate;

		[XmlAttribute]
		public double SuppressionHeatFactor;

		[XmlAttribute]
		public double SuppressionIRFactor;
	}
}
