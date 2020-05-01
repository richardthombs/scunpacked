using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class DamageResistanceEntry
	{
		[XmlAttribute]
		public double Multiplier;

		[XmlAttribute]
		public int Threshold;
	}
}