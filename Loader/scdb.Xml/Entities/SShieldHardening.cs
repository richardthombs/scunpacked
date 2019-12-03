using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SShieldHardening
	{
		[XmlAttribute]
		public double Factor;

		[XmlAttribute]
		public double Duration;

		[XmlAttribute]
		public double Cooldown;
	}
}