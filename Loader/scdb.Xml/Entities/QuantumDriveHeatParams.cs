using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class QuantumDriveHeatParams
	{
		[XmlAttribute]
		public double inFlightThermalEnergyDraw;

		[XmlAttribute]
		public double postRampDownThermalEnergyDraw;

		[XmlAttribute]
		public double preRampUpThermalEnergyDraw;

		[XmlAttribute]
		public double rampDownThermalEnergyDraw;

		[XmlAttribute]
		public double rampUpThermalEnergyDraw;
	}
}
