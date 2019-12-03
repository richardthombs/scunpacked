using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class DamageResistanceEntry
	{
		[XmlAttribute]
		public double Multiplier;

		[XmlAttribute]
		public int Threshold;
	}
}