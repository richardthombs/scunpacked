using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SShieldResistance
	{
		[XmlAttribute]
		public double Max;

		[XmlAttribute]
		public double Min;

	}
}