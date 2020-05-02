using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class EntityComponentHeatConnection
	{
		[XmlAttribute]
		public double Mass;

		[XmlAttribute]
		public double MaxCoolingRate;

		[XmlAttribute]
		public double MaxTemperature;

		[XmlAttribute]
		public double MinTemperature;

		[XmlAttribute]
		public double MisfireMaxTemperature;

		[XmlAttribute]
		public double MisfireMinTemperature;

		[XmlAttribute]
		public double OverclockThresholdMaxHeat;

		[XmlAttribute]
		public double OverclockThresholdMinHeat;

		[XmlAttribute]
		public double OverheatTemperature;

		[XmlAttribute]
		public double OverpowerHeat;

		[XmlAttribute]
		public double RecoveryTemperature;

		[XmlAttribute]
		public double SpecificHeatCapacity;

		[XmlAttribute]
		public double StartCoolingTemperature;

		[XmlAttribute]
		public double SurfaceArea;

		[XmlAttribute]
		public double TemperatureToIR;

		[XmlAttribute]
		public double ThermalConductivity;

		[XmlAttribute]
		public double ThermalEnergyBase;

		[XmlAttribute]
		public double ThermalEnergyDraw;
	}
}
