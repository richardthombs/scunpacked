using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SHealthComponentParams
	{
		public DamageResistances DamageResistances;

		[XmlAttribute]
		public double Health;
	}
}
