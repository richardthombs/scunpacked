using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SShieldAbsorption
	{
		[XmlAttribute]
		public double Max;

		[XmlAttribute]
		public double Min;

	}
}