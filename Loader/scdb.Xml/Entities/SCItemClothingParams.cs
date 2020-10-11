using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemClothingParams
	{
		public TemperatureResistance TemperatureResistance;
	}

	public class TemperatureResistance
	{
		[XmlAttribute]
		public string MinResistance;

		[XmlAttribute]
		public string MaxResistance;
	}
}
