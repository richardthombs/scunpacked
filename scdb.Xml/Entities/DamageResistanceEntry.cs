using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class DamageResistanceEntry
	{
		[XmlAttribute]
		public double Multiplier { get; set; }

		[XmlAttribute]
		public int Threshold { get; set; }
	}
}