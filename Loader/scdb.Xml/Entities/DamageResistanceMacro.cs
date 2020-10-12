using System.Xml;
using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class DamageResistanceMacro
	{
		[XmlAttribute]
		public string __ref;

		public DamageResistance damageResistance;
		public DamageInfo damageInfo;
	}
}
