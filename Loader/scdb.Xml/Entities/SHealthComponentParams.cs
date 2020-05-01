using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SHealthComponentParams
	{
		[XmlAttribute]
		public double Health;

		public DamageResistances DamageResistances;
	}
}