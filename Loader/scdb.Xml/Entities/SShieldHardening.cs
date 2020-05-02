using System.Xml.Serialization;

namespace Loader.SCDb.Xml.Entities
{
	public class SShieldHardening
	{
		[XmlAttribute]
		public double Cooldown;

		[XmlAttribute]
		public double Duration;

		[XmlAttribute]
		public double Factor;
	}
}
