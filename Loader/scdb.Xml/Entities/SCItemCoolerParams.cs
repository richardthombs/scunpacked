using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
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