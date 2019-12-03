using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SHealthComponentParams
	{
		[XmlAttribute]
		public double Health;

		public DamageResistances DamageResistances;
	}
}