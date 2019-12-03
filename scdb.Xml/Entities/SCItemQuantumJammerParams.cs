using System.Xml.Serialization;

namespace scdb.Xml.Entities
{
	public class SCItemQuantumJammerParams
	{
		[XmlAttribute]
		public double jammerRange;

		[XmlAttribute]
		public double maxPowerDraw;

		[XmlAttribute]
		public double greenZoneCheckRange;

		[XmlAttribute]
		public string setJammerSwitchOn;

		[XmlAttribute]
		public string setJammerSwitchOff;
	}
}