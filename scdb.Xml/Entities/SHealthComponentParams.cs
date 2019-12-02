using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SHealthComponentParams
	{
		[XmlAttribute]
		public double Health { get; set; }

		public DamageResistances DamageResistances { get; set; }
	}
}