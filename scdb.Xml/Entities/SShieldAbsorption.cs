using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SShieldAbsorption
	{
		[XmlAttribute]
		public double Max;

		[XmlAttribute]
		public double Min;

	}
}